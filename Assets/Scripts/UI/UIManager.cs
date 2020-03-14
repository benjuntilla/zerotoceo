using UnityEngine;

namespace UI 
{
    public class UIManager : MonoBehaviour
    {
        private LevelManager _levelManager;
        private MinigameManager _minigameManager;
        private MenuFull _menuFull;

        public GameObject mainMenuUI;

        void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _menuFull = FindObjectOfType<MenuFull>();
            _minigameManager = FindObjectOfType<MinigameManager>();

            TriggerApplicableMenus();
        }
        
        private void TriggerApplicableMenus()
        {
            mainMenuUI.SetActive(_levelManager.currentLevelType == LevelManager.LevelType.Menu);
            if (_levelManager.levelIndex == 1 && !SaveManager.loadFlag)
                _menuFull.Trigger("opening");
            else if (_levelManager.currentLevelType == LevelManager.LevelType.Minigame && MinigameManager.minigameId != "")
                _menuFull.Trigger(MinigameManager.minigameName);
            else if (_levelManager.currentLevelType == LevelManager.LevelType.Minigame && MinigameManager.minigameId == "")
                _menuFull.Trigger(_minigameManager.ResolveEmptyMinigame()); 
        }
    }
}
