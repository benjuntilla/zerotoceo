using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : UIObject
    {
        private void PauseGame ()
        {
            Enable();
            Time.timeScale = 0f;
        }

        public void ResumeGame ()
        {
            Disable();
            Time.timeScale = 1f;
        }

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
            {
                if (Time.timeScale == 0f)
                    ResumeGame();
                else
                    PauseGame();
            }
        }
    }
}
