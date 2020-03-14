using System;
using UnityEngine;

namespace UI
{
    public class Fade : UIObject
    {
        public Animator animator;

        void Start()
        {
            Enable();
        }

        public void FadeOutThenAction(Action action)
        {
            StopAllCoroutines();
            StartCoroutine(Helper.PlayOutThenAction(animator, action));
        }
        
        public void FadeInThenAction(Action action)
        {
            StopAllCoroutines();
            StartCoroutine(Helper.PlayInThenAction(animator, action));
        }
    }
}
