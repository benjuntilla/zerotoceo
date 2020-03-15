using UnityEngine;

namespace UI
{
    public class MainMenu : UIObject
    {
        private LevelManager _levelManager;
        
        void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            if (_levelManager.currentLevelType == LevelManager.LevelType.Menu)
                Enable();
            else
                Disable();
        }
    }
}