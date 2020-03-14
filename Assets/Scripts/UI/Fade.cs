using System;
using UnityEngine;

namespace UI
{
    public class Fade : UIObject
    {
        public Animator animator;

        void Start()
        {
            FadeOutThenAction(() => {});
        }

        public void FadeOutThenAction(Action action)
        {
            Enable();
            StopAllCoroutines();
            StartCoroutine(Helper.PlayOutThenAction(animator, () =>
            {
                Disable();
                action();
            }));
        }
        
        public void FadeInThenAction(Action action)
        {
            Enable();
            StopAllCoroutines();
            StartCoroutine(Helper.PlayInThenAction(animator, action));
        }
    }
}
