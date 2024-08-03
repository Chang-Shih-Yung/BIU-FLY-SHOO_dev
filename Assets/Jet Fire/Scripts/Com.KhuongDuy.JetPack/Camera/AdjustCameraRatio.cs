using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.JetPack
{
    /// <summary>
    /// Adjust the ratio of the camera compared to the size of the device screen
    /// </summary>
    public class AdjustCameraRatio : MonoBehaviour
    {
        private new Camera camera;

        [SerializeField]
        private TargetScreenSize sizeScreenTarget;

        // Constructor
        private AdjustCameraRatio() { }

        // Behaviour messages
        void Awake()
        {
            camera = GetComponent<Camera>();
        }

        // Behaviour messages
        void Start()
        {
            AdjustRatio();
        }

        private void AdjustRatio()
        {
            float targetAspect = sizeScreenTarget.width / sizeScreenTarget.height;
            float screenAspect = (float)Screen.width / (float)Screen.height;
            float scaleHeight = screenAspect / targetAspect;

            if (scaleHeight < 1.0f)
            {
                Rect rect = camera.rect;
                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f;
                camera.rect = rect;
            }
            else
            {
                float scaleWidth = 1.0f / scaleHeight;

                Rect rect = camera.rect;
                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                camera.rect = rect;
            }
        }
    }

    [System.Serializable]
    public struct TargetScreenSize
    {
        public float width;
        public float height;
    }
}
