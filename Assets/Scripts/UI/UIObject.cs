using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIObject : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        
        protected bool blocksRaycasts = true;
        
        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void Disable()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void Enable()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = blocksRaycasts;
        }
    }
}