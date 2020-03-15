using DG.Tweening;
using System;

namespace UI
{
    public class Fadeable : UIObject
    {
        public float fadeDuration = 0.5f;

        public void FadeIn(Action cb = null)
        {
            Enable();
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
            {
                cb?.Invoke();
            });
        }

        public void FadeOut(Action cb = null)
        {
            Enable();
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
            {
                Disable();
                cb?.Invoke();
            });
        }
    }
}
