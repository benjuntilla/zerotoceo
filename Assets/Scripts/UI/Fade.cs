using System;
using UnityEngine;

namespace UI
{
    public class Fade : UIObject
    {
        public Animator animator;

        void Start()
        {
            blocksRaycasts = false;
            Enable();
        }

        public void FadeOutThenAction(Action action)
        {
            blocksRaycasts = true;
            StopAllCoroutines();
            StartCoroutine(Helper.PlayOutThenAction(animator, () =>
            {
                blocksRaycasts = false;
                action();
            }));
        }
        
        public void FadeInThenAction(Action action)
        {
            blocksRaycasts = true;
            StopAllCoroutines();
            StartCoroutine(Helper.PlayInThenAction(animator, action));
        }
    }
}
