using UnityEngine;

namespace UI 
{
    public class CustomCursor : MonoBehaviour
    {
        public Texture2D cursorTexture;

        private LevelManager _levelManager;
        private DialogueManager _dialogueManager;
        
        void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _dialogueManager = FindObjectOfType<DialogueManager>();
        }

        void Update()
        {
            if (_levelManager.currentLevelType == LevelManager.LevelType.Menu ||
                _levelManager.levelName == "Minigame_Coin" ||
                Time.timeScale == 0 ||
                (_levelManager.currentLevelType == LevelManager.LevelType.Level && _dialogueManager.currentDialogueName != ""))
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

