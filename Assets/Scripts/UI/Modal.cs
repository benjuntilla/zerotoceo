using TMPro;
using UnityEngine;

namespace UI
{
    public class Modal : UIObject
    {
        public TextMeshProUGUI modalText;
        public GameObject primaryButton, secondaryButton;
        
        private string _action;
        private TextMeshProUGUI _primaryButtonText, _secondaryButtonText;
        private HUD _hud;
        private PauseMenu _pauseMenu;
        private SaveManager _saveManager;
        private MinigameManager _minigameManager;
        private Fade _fade;
        private LevelManager _levelManager;

        void Start()
        {
            _hud = FindObjectOfType<HUD>();
            _pauseMenu = FindObjectOfType<PauseMenu>();
            _saveManager = FindObjectOfType<SaveManager>();
            _minigameManager = FindObjectOfType<MinigameManager>();
            _levelManager = FindObjectOfType<LevelManager>();
            _fade = FindObjectOfType<Fade>();
            _primaryButtonText = primaryButton.GetComponentInChildren<TextMeshProUGUI>();
            _secondaryButtonText = secondaryButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Trigger (string id)
        {
            // !! Default options !!
            Time.timeScale = 0;
            modalText.alignment = TextAlignmentOptions.Justified;
            primaryButton.SetActive(true);
            secondaryButton.SetActive(true);
            _primaryButtonText.SetText("yes");
            _secondaryButtonText.SetText("no");
            // !! Default options !!
            _action = id;
            Enable();
            switch (id)
            {
                case "minigame":
                    modalText.SetText("Do you want to start the minigame?");
                    break;
                case "minigameFail":
                    modalText.alignment = TextAlignmentOptions.Left;
                    _primaryButtonText.SetText("ok");
                    secondaryButton.SetActive(false);
                    modalText.SetText($"You failed the minigame. (Lost 1 life)");
                    break;
                case "minigamePass":
                    modalText.alignment = TextAlignmentOptions.Left;
                    _primaryButtonText.SetText("ok");
                    secondaryButton.SetActive(false);
                    modalText.SetText($"You passed the minigame. (Gained {_minigameManager.minigamePoints[MinigameManager.minigameId]} points)");
                    break;
                case "save":
                    modalText.SetText("Are you sure you want to save? This will overwrite current save data!");
                    break;
                case "load":
                    modalText.SetText("Load save data?");
                    break;
                case "loadFromMain":
                    modalText.SetText("Load save data?");
                    break;
                case "new":
                    modalText.SetText("Are you sure you want to start a new game? This will NOT load previous save data!");
                    break;
                case "quit":
                    modalText.SetText("Are you sure you want to quit? This will discard unsaved game data!");
                    break;
                case "main":
                    modalText.SetText("Are you sure you want to exit to the main menu? This will discard unsaved game data!");
                    break;
            }
        }
        
        public void Confirm ()
        {
            Time.timeScale = 1;
            Disable();
            switch (_action)
            {
                case "minigame":
                    _hud.Disable();
                    _minigameManager.InitializeMinigame();
                    _fade.FadeInThenAction(() =>
                    {
                        _levelManager.LoadLevel(MinigameManager.minigameName);
                    });
                    break;
                case "minigameFail":
                case "minigamePass":
                    _fade.FadeInThenAction(() =>
                    {
                        _hud.Enable();
                        _levelManager.LoadSavedLevel();
                    });
                    break;
                case "save":
                    _pauseMenu.ResumeGame();
                    _saveManager.Save();
                    break;
                case "load":
                    _pauseMenu.ResumeGame();
                    _saveManager.Load();
                    break;
                case "loadFromMain":
                    _pauseMenu.ResumeGame();
                    Disable();
                    _fade.FadeInThenAction(() =>
                    {
                        _levelManager.LoadSavedLevel();
                    });
                    break;
                case "new":
                    _pauseMenu.ResumeGame();
                    _fade.FadeInThenAction(() =>
                    {
                        _levelManager.LoadLevel(1);
                    });
                    break;
                case "main":
                    _pauseMenu.ResumeGame();
                    _fade.FadeInThenAction(() =>
                    {
                        _levelManager.LoadLevel(0);
                    });
                    break;
                case "quit":
                    Application.Quit();
                    break;
            }
        }
    }
}
