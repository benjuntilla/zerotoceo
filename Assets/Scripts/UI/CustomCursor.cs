using UnityEngine;

namespace UI 
{
    public class CustomCursor : MonoBehaviour
    {
        public Texture2D cursorTexture;

        private LevelManager _levelManager;
        private DialogueManager _dialogueManager;
        private Fade _fade;
        private MenuFull _menuFull;
        private PauseMenu _pauseMenu;
        private Modal _modal;
        
        void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _dialogueManager = FindObjectOfType<DialogueManager>();
            _fade = FindObjectOfType<Fade>();
            _menuFull = FindObjectOfType<MenuFull>();
            _pauseMenu = FindObjectOfType<PauseMenu>();
            _modal = FindObjectOfType<Modal>();
        }

        void Update()
        {
            if (_levelManager.currentLevelType == LevelManager.LevelType.Menu ||
                (_levelManager.currentLevelType == LevelManager.LevelType.Level && _dialogueManager.currentDialogueName != "") ||
                _levelManager.levelName == "Minigame_Coin" ||
                (_fade.canvas.enabled && _fade.canvasGroup.alpha == 1) ||
                (_menuFull.canvas.enabled && _menuFull.canvasGroup.alpha == 1) ||
                _pauseMenu.canvas.enabled ||
                _modal.canvas.enabled)
            {
                Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }
        }
    }
}

