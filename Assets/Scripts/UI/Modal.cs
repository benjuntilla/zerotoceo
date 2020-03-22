using TMPro;
using UnityEngine;

namespace UI
{
    public class Modal : UIObject
    {
        public TextMeshProUGUI modalText;
        public GameObject primaryButton, secondaryButton;
        public string action { get; private set; }
        
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

        public void Trigger(string id)
        {
            Enable();
            Time.timeScale = 0;
            action = id;
            
            modalText.alignment = TextAlignmentOptions.Justified;
            primaryButton.SetActive(true);
            secondaryButton.SetActive(true);
            _primaryButtonText.SetText("yes");
            _secondaryButtonText.SetText("no");
            switch (id)
            {
                case "minigame":
                    modalText.SetText("Do you want to start the minigame?");
                    break;
                case "minigameFail":
                    Time.timeScale = 1f;
                    modalText.alignment = TextAlignmentOptions.Left;
                    _primaryButtonText.SetText("ok");
                    secondaryButton.SetActive(false);
                    modalText.SetText($"You failed the minigame. (Lost 1 life)");
                    break;
                case "minigamePass":
                    Time.timeScale = 1f;
                    modalText.alignment = TextAlignmentOptions.Left;
                    _primaryButtonText.SetText("ok");
                    secondaryButton.SetActive(false);
                    modalText.SetText(
                        $"You passed the minigame. (Gained {_minigameManager.CurrentPotentialPointGain()} points)");
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
                    modalText.SetText(
                        "Are you sure you want to start a new game? This will NOT load previous save data!");
                    break;
                case "quit":
                    modalText.SetText("Are you sure you want to quit? This will discard unsaved game data!");
                    break;
                case "main":
                    modalText.SetText(
                        "Are you sure you want to exit to the main menu? This will discard unsaved game data!");
                    break;
                case "softLock":
                    modalText.SetText("You have lost too many points for the level to continue. Reverting to the last save.");
                    _primaryButtonText.SetText("Continue");
                    secondaryButton.SetActive(false);
                    break;
            }
        }

        public void Confirm ()
        {
            switch (action)
            {
                case "minigame":
                    _hud.Disable();
                    _minigameManager.PrepareMinigame();
                    _fade.FadeIn(() =>
                    {
                        _levelManager.LoadLevel(MinigameManager.minigameInfo.name);
                    });
                    break;
                case "minigameFail":
                case "minigamePass":
                    _fade.FadeIn(() =>
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
                    _fade.FadeIn(() =>
                    {
                        _levelManager.LoadSavedLevel();
                    });
                    break;
                case "new":
                    _pauseMenu.ResumeGame();
                    _fade.FadeIn(() =>
                    {
                        _levelManager.LoadLevel(1);
                    });
                    break;
                case "main":
                    _pauseMenu.ResumeGame();
                    _fade.FadeIn(() =>
                    {
                        _levelManager.LoadLevel(0);
                    });
                    break;
                case "quit":
                    Application.Quit();
                    break;
                case "softLock":
                    _fade.FadeIn(() =>
                    {
                        _levelManager.LoadSavedLevel();
                    });
                    break;
            }
            action = null;
            Time.timeScale = 1;
            Disable();
        }

        public void Dismiss()
        {
            action = null;
            Time.timeScale = 1;
            Disable();
        }
    }
}
