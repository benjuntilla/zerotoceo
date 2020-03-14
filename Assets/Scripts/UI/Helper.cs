using TMPro;
using System.Collections;

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
    } 
}
