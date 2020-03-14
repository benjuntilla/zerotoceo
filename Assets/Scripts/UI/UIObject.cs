using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasGroup), typeof(GraphicRaycaster))]
    public class UIObject : MonoBehaviour
    {
        public Canvas canvas { get; private set; }
        public CanvasGroup canvasGroup { get; private set; }
        
        protected bool blocksRaycasts = true;
        
        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void Disable()
        {
            canvas.enabled = false;
        }
        
        public void Enable()
        {
            canvas.enabled = true;
        }

        protected virtual void Update()
        {
            canvasGroup.blocksRaycasts = canvas.enabled && blocksRaycasts;
        }
    }
}