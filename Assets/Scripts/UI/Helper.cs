using TMPro;
using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public static class Helper
    {
        public static void PlayIn(Animator animator)
        {
            animator.Play(Animator.StringToHash("In"));
        }
        
        public static void PlayOut(Animator animator)
        {
            animator.Play(Animator.StringToHash("Out"));
        }
        
        public static IEnumerator PlayInThenAction(Animator animator, Action action)
        {
            PlayIn(animator);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("In")) // Wait until the animation begins playing
                yield return null;
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("In")) // Wait until the animation transitions to another
                yield return null;
            action();
        }
    
        public static IEnumerator PlayOutThenAction(Animator animator, Action action)
        {
            PlayOut(animator);
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Out")) // Wait until the animation begins playing
                yield return null;
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("Out")) // Wait until the animation transitions to another
                yield return null;
            action();
        }
        
        public static IEnumerator TypeText(TextMeshProUGUI textObject, string text)
        {
            var t = "";
            textObject.SetText("");
            foreach (var letter in text.ToCharArray())
            {
                t += letter;
                textObject.SetText(t);
                yield return null; // Wait a frame
            }
        }
    } 
}
