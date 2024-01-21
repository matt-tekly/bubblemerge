using System.Collections;
using Tekly.Common.Utils.Tweening;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// A bubble that can merge with other bubbles
	/// </summary>
	public class Bubble : MonoBehaviour
	{
		[SerializeField] private GameConfig m_config;
		
		[SerializeField] private SpriteRenderer m_renderer;
		[SerializeField] private Rigidbody2D m_body;
		[SerializeField] private BubbleManager m_manager;
		
		/// <summary>
		/// Is this Bubble being merged into another bubble.
		/// When a bubble is done merging into another bubble it is destroyed.
		/// It cannot merge or grow while this is happening
		/// </summary>
		public bool IsMerging { get; set; }
		
		/// <summary>
		/// Indicates this bubble is growing into a larger bubble.
		/// It cannot merge or grow while this is happening
		/// </summary>
		public bool IsGrowing { get; set; }
		
		/// <summary>
		/// Lower values means the Bubble is older
		/// </summary>
		public float BirthTime { get; private set; }
		
		public bool HasCollided { get; private set; }

		public int Size { get; set; }
		
		private void OnEnable()
		{
			BirthTime = Time.realtimeSinceStartup;
			transform.localScale = GetScale();
			ApplyGraphics();

			m_body.simulated = false;
		}

		private Vector3 GetScale()
		{
			var size = m_config.BubbleConfig.BaseSize * m_config.BubbleConfig.SizeRatios[Size];
			return new Vector3(size, size, size);
		}

		private void ApplyGraphics()
		{
			var colors = m_config.BubbleConfig.Colors;
			m_renderer.color = colors[Mathf.Min(Size, colors.Length - 1)];
			m_body.mass = m_config.BubbleConfig.SizeRatios[Size];
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			HasCollided = true;
			HandleCollision(other);
		}

		private void OnCollisionStay2D(Collision2D other)
		{
			HandleCollision(other);
		}

		private void HandleCollision(Collision2D other)
		{
			if (IsMerging || IsGrowing) {
				return;
			}
			
			var otherBubble = other.gameObject.GetComponent<Bubble>();
			
			if (otherBubble == null || otherBubble.Size != Size || otherBubble.IsMerging || otherBubble.IsGrowing) {
				return;
			}

			if (BirthTime < otherBubble.BirthTime) {
				otherBubble.MergeInto(this);
				Grow();	
			} else {
				MergeInto(otherBubble);
				otherBubble.Grow();
			}
		}

		private void Grow()
		{
			StartCoroutine(DoGrow());
		}

		private void OnDestroy()
		{
			m_manager.BubbleDestroyed(this);
		}

		private void MergeInto(Bubble bubble)
		{
			IsMerging = true;
			StartCoroutine(DoMerge(bubble));
		}

		public void Fall(Vector3 position)
		{
			transform.position = position;
			m_body.simulated = true;
			m_body.velocityY = -m_config.BubbleFallSpeed;
		}
		
		public IEnumerator Appear(Vector3 position)
		{
			transform.position = position;
			
			m_body.simulated = false;

			var startScale = Vector3.zero;
			var endScale = transform.localScale;
			
			var timer = 0f;
			while (timer < m_config.BubbleAppearTween.Duration) {
				var ratio = Easing.Evaluate(timer / m_config.BubbleAppearTween.Duration, m_config.BubbleAppearTween.Ease);
				transform.localScale = Vector3.Lerp(startScale, endScale, ratio);

				timer += Time.deltaTime;
				yield return null;
			}

			transform.localScale = endScale;

			yield return null;
		}

		private IEnumerator DoMerge(Bubble bubble)
		{
			m_body.simulated = false;
			
			var destination = bubble.transform.position;
			var position = transform.position;

			var startScale = transform.localScale;
			var endScale = new Vector3(1f, 1f, 1f);

			var startColor = m_renderer.color;
			var endColor = m_renderer.color;
			endColor.a = 0;

			var timer = 0f;

			while (timer < m_config.MergeTime) {
				var ratio = timer / m_config.MergeTime;

				transform.position = Vector3.Lerp(position, destination, ratio);
				transform.localScale = Vector3.Lerp(startScale, endScale, ratio);
				m_renderer.color = Color.Lerp(startColor, endColor, ratio);

				timer += Time.deltaTime;
				
				yield return null;
			}
			
			Destroy(gameObject);
		}

		private IEnumerator DoGrow()
		{
			IsGrowing = true;
			
			Size++;
			
			ApplyGraphics();
			
			var timer = 0f;
			var t = transform;

			var scale = transform.localScale;
			var newScale = GetScale();

			while (timer <= m_config.GrowTime) {
				t.localScale = Vector3.Lerp(scale, newScale, timer / m_config.GrowTime);
				timer += Time.deltaTime;
				yield return null;
			}

			t.localScale = newScale;
			IsGrowing = false;
		}
	}
}