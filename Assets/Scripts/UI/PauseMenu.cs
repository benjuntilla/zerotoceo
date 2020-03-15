using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : UIObject
    {
        public bool isPaused { get; private set; }

        private Modal _modal;

        private void Start()
        {
            _modal = FindObjectOfType<Modal>();
        }

        private void PauseGame ()
        {
            Enable();
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void ResumeGame ()
        {
            Disable();
            Time.timeScale = 1f;
            isPaused = false;
        }

        protected override void Update()
        {
            base.Update();
            if (!Input.GetKeyDown(KeyCode.Escape) || SceneManager.GetActiveScene().buildIndex == 0) return;
            if (isPaused && _modal.action == null)
                ResumeGame();
            else if (_modal.action == null && Time.timeScale == 1f)
                PauseGame();
        }
    }
}
