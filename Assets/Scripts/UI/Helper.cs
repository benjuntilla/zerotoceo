using TMPro;
using System.Collections;
using UnityEngine;

namespace UI
{
    public static class Helper
    {
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

        public static void DisableChildren(GameObject parent)
        {
            foreach (Transform child in parent.transform)
                 child.gameObject.SetActive(false);
        }
    } 
}
