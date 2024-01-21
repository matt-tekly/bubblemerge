using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{

	/// <summary>
	///  Detects if the container is full
	/// </summary>
	public class FullSensor : MonoBehaviour
	{
		[SerializeField] private GameConfig m_config;
		[SerializeField] private GameObject m_bar
			;
		private List<Bubble> m_bubbles = new List<Bubble>();

		private float m_timer;
		
		private void Update()
		{
			if (!m_bubbles.Any(x => x.HasCollided)) {
				m_timer = 0;
				m_bar.SetActive(false);
				return;
			}

			m_bar.SetActive(true);
			m_timer += Time.deltaTime;

			if (m_timer > m_config.FullTimeLose) {
				Debug.Log("YOU LOSE");
			}
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			var bubble = other.gameObject.GetComponent<Bubble>();
			if (bubble == null) {
				return;
			}
			
			m_bubbles.Add(bubble);
		}

		private void OnTriggerExit2D(Collider2D other)
		{
			var bubble = other.gameObject.GetComponent<Bubble>();
			if (bubble == null) {
				return;
			}
			
			m_bubbles.Remove(bubble);
		}
	}
}