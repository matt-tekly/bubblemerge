using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	/// <summary>
	/// Handles the placement and creation of bubbles
	/// </summary>
	public class BubbleManager : MonoBehaviour
	{
		[SerializeField] private Bubble[] m_bubbles;
		[SerializeField] private Camera m_camera;
		[SerializeField] private GameConfig m_gameConfig;
		
		private bool m_canPlaceBubble;

		private Bubble m_nextBubble;
		private float m_nextBubbleSize;
		private int m_providedBubbles;

		private List<Bubble> m_allBubbles = new List<Bubble>();
		
		private void Awake()
		{
			foreach (var bubble in m_bubbles) {
				bubble.gameObject.SetActive(false);
			}

			StartCoroutine(ShowBubble());
		}

		private void Update()
		{
			if (Input.GetMouseButtonUp(0) && m_canPlaceBubble) {
				StartCoroutine(PlaceBubble());
			}

			if (Input.GetMouseButton(0) && m_canPlaceBubble) {
				m_nextBubble.transform.position = GetBubblePositionFromMouse();
			}
		}

		private Vector2 GetBubblePositionFromMouse()
		{
			Vector2 position = m_camera.ScreenToWorldPoint(Input.mousePosition);
			position.y = m_gameConfig.StartHeight;
			
			var left = m_gameConfig.BubbleProviderConfig.WorldLeft + m_nextBubbleSize * 0.5f;
			var right = m_gameConfig.BubbleProviderConfig.WorldRight - m_nextBubbleSize * 0.5f;
			
			position.x = Mathf.Clamp(position.x, left, right);
			
			return position;
		}
		
		private Bubble CreateBubble()
		{
			var index = Random.Range(0, m_gameConfig.BubbleProviderConfig.SizeMax);
			
			if (m_providedBubbles < m_gameConfig.BubbleProviderConfig.StartingSizes.Length) {
				index = m_gameConfig.BubbleProviderConfig.StartingSizes[m_providedBubbles];
			}
			
			var bubble = m_bubbles[Random.Range(0, m_bubbles.Length)];
			var instance = Instantiate(bubble);
			instance.Size = index;

			m_nextBubbleSize = m_gameConfig.BubbleConfig.BaseSize * m_gameConfig.BubbleConfig.SizeRatios[index];

			m_providedBubbles++;
			
			return instance;
		}

		private IEnumerator ShowBubble()
		{
			m_canPlaceBubble = false;
				
			m_nextBubble = CreateBubble();
			m_nextBubble.gameObject.SetActive(true);
			yield return m_nextBubble.Appear(new Vector3(0, m_gameConfig.StartHeight, 0));

			m_canPlaceBubble = true;
		}
		
		private IEnumerator PlaceBubble()
		{
			m_canPlaceBubble = false;

			var position = GetBubblePositionFromMouse();
			m_nextBubble.Fall(position);

			yield return new WaitForSeconds(1f);
			yield return ShowBubble();
		}

		public void BubbleDestroyed(Bubble bubble)
		{
			m_allBubbles.Remove(bubble);
		}
	}
}