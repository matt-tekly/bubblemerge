using UnityEngine;

namespace Game.Cameras
{
    public class GameCamera : MonoBehaviour
    {
        [SerializeField] private Camera m_camera;
        [SerializeField] private RectTransform m_boundsTransform;
        
        void Update()
        {
            UpdateForBounds();
        }
        
        private void UpdateForBounds()
        {
            var rect = m_boundsTransform.rect;
            transform.position = m_boundsTransform.position;

            var screenRatio = Screen.width / (float) Screen.height;
            var targetRatio = rect.size.x / rect.size.y;

            if (screenRatio >= targetRatio) {
                m_camera.orthographicSize = rect.size.y / 2;
            } else {
                var differenceInSize = targetRatio / screenRatio;
                m_camera.orthographicSize = rect.size.y / 2 * differenceInSize;
            }
        }
    }
}
