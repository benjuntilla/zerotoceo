using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Scrolling : MonoBehaviour
    {
        public Vector2 velocity = new Vector2(1, 1);

        private RawImage _rawImage;
        private Vector2 _offset = new Vector2(0, 0);

        void Awake()
        {
            _rawImage = GetComponent<RawImage>();
        }

        void Update()
        {
            _offset = new Vector2(velocity.x * Time.realtimeSinceStartup, velocity.y * Time.realtimeSinceStartup);
            _rawImage.uvRect = new Rect(_offset, new Vector2(_rawImage.uvRect.width, _rawImage.uvRect.height));
        }
    }
}