using UnityEngine;
using UnityEngine.UI;

namespace Game
{
	public class RawImageMover : MonoBehaviour
	{
		[SerializeField] private RawImage m_rawImage;
		[SerializeField] private Vector2 m_scrollSpeed;

		private Vector2 m_scrollPos;

		private void Update()
		{
			m_scrollPos += m_scrollSpeed * Time.deltaTime;
			var width = m_rawImage.uvRect.width;
			var height = m_rawImage.uvRect.height;
			
			m_rawImage.uvRect = new Rect(m_scrollPos.x, m_scrollPos.y, width, height);
		}
	
#if UNITY_EDITOR
		private void OnValidate()
		{
			if (m_rawImage == null) {
				m_rawImage = GetComponent<RawImage>();
			}
		}
#endif
	}
}