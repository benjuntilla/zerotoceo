using UnityEngine;

namespace UI
{
    public class PanelGroup : UIObject
    {
        public void OpenPanel(GameObject target)
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
            target.gameObject.SetActive(true);
        }
    }
}