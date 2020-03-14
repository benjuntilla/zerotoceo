using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup), typeof(GraphicRaycaster))]
    public class UIObject : MonoBehaviour
    {
        private Canvas _canvas;
        private CanvasGroup _canvasGroup;
        
        protected bool blocksRaycasts = true;
        
        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void Disable()
        {
            _canvas.enabled = false;
        }
        
        public void Enable()
        {
            _canvas.enabled = true;
        }

        protected virtual void Update()
        {
            _canvasGroup.blocksRaycasts = _canvas.enabled && blocksRaycasts;
        }
    }
}