using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuFull : Fadeable
    {
        public GameObject playerTwo, playerThree, playerFour, futureToken, businessToken, leaderToken, americaToken, images;
        public TextMeshProUGUI titleText, bodyText, buttonText, controlsText;
        [TextArea(3, 10)]
        public string closingMenuText, openingMenuText, gameOverText, grandmaMinigameText, coinMinigameText, trashMinigameText;

        private bool _triggeredDeathMenu;
        private int _counter;
        private string _action;
        private Fade _fade;
        private LevelManager _levelManager;
        private MinigameManager _minigameManager;
        private Coroutine _typeCoroutine;
        private Player _player;
        private SaveManager _saveManager;
        
        void Start()
        {
            _fade = FindObjectOfType<Fade>();
            _levelManager = FindObjectOfType<LevelManager>();
            _minigameManager = FindObjectOfType<MinigameManager>();
            _player = FindObjectOfType<Player>();
            _saveManager = FindObjectOfType<SaveManager>();
            
            TriggerAppropriateMenu();
        }

        private void TriggerAppropriateMenu()
        {
            if (_levelManager.levelIndex == 1 && !_saveManager.loaded)
                Trigger("opening");
            else if (_levelManager.currentLevelType == LevelManager.LevelType.Minigame && MinigameManager.minigameInfo.id != null)
                Trigger(MinigameManager.minigameInfo.name);
            else if (_levelManager.currentLevelType == LevelManager.LevelType.Minigame && MinigameManager.minigameInfo.id == null)
                Trigger(_minigameManager.ResolveEmptyMinigame()); 
        }

        public void Trigger(string id)
        {
            Enable();
            _counter = 0;
            _action = id;
            switch (id)
            {
                case "opening":
                    _fade.FadeOut(() =>
                    {
                        Time.timeScale = 0f;
                    });
                    titleText.SetText("The beginning");
                    _typeCoroutine = StartCoroutine(Helper.TypeText(bodyText, openingMenuText));
                    buttonText.SetText("Continue");
                    break;
                case "death":
                    if (_triggeredDeathMenu) break;
                    _triggeredDeathMenu = true;
                    titleText.SetText("Game Over");
                    _typeCoroutine = StartCoroutine(Helper.TypeText(bodyText, gameOverText));
                    buttonText.SetText("Main Menu");
                    break;
                case "closing":
                    Time.timeScale = 1f;
                    FadeIn(() =>
                    {
                        Time.timeScale = 0f;
                    });                
                    titleText.SetText("You Beat the Game!");
                    _typeCoroutine = StartCoroutine(Helper.TypeText(bodyText, closingMenuText));
                    buttonText.SetText("Main Menu");
                    break;
                case "levelEnd":
                    FadeIn(() =>
                    {
                        Time.timeScale = 0f;
                    });
                    titleText.SetText("Level Up!");
                    bodyText.alignment = TextAlignmentOptions.Center;
                    bodyText.alignment = TextAlignmentOptions.Top;
                    bodyText.SetText($"Good job completing level {_levelManager.levelIndex}!");
                    buttonText.SetText("Continue");
                    break;
                case "Minigame_Grandma":
                    titleText.SetText("Minigame: Cross the street");
                    _typeCoroutine = StartCoroutine(Helper.TypeText(bodyText, grandmaMinigameText));
                    buttonText.SetText("Continue");
                    break;
                case "Minigame_Trash":
                    titleText.SetText("Minigame: Collect the trash");
                    _typeCoroutine = StartCoroutine(Helper.TypeText(bodyText, trashMinigameText));
                    buttonText.SetText("Continue");
                    break;
                case "Minigame_Coin":
                    titleText.SetText("Minigame: Sort the coins");
                    _typeCoroutine = StartCoroutine(Helper.TypeText(bodyText, coinMinigameText));
                    buttonText.SetText("Continue");
                    break;
            }
        }

        public void Continue()
        {
            _counter++;
            if (_action == "levelEnd")
            {
                Helper.DisableChildren(images);
                switch (_counter)
                {
                    case 1: // Scoreboard page
                        titleText.SetText("Scoreboard");
                        bodyText.SetText(
                            $"\n" +
                            $"Dialogue bonus: {_levelManager.scoreboard["dialogueBonus"]}\n" +
                            $"Dialogue penalty: {_levelManager.scoreboard["dialoguePenalty"]}\n" +
                            $"Minigame bonus: {_levelManager.scoreboard["minigameBonus"]}\n" +
                            $"Total points: {_player.points}\n"
                        );
                        if (_levelManager.levelIndex == 4) // Skip the clothing slide on level 4 since it doesnt exist
                            _counter++;
                        break;
                    case 2: // Show new clothing
                        titleText.SetText("Got new clothing!");
                        bodyText.alignment = TextAlignmentOptions.Left;
                        bodyText.SetText("");
                        switch (_levelManager.levelIndex)
                        {
                            case 1: // Level has not actually increased yet, so the desired value minus one must be used.
                                playerTwo.gameObject.SetActive(true);
                                break;
                            case 2:
                                playerThree.gameObject.SetActive(true);
                                break;                    
                            case 3:
                                playerFour.gameObject.SetActive(true);
                                break;
                        }
                        break;
                    case 3: // Show token
                        titleText.SetText("Received a BAA token!");
                        bodyText.SetText("");
                        switch (_levelManager.levelIndex)
                        {
                            case 1: // Level has not actually increased yet, so the desired value minus one must be used.
                                futureToken.gameObject.SetActive(true);
                                break;
                            case 2:
                                businessToken.gameObject.SetActive(true);
                                break;                    
                            case 3:
                                leaderToken.gameObject.SetActive(true);
                                break;
                            case 4:
                                americaToken.gameObject.SetActive(true);
                                break;
                        }
                        break;
                    case 4: // Either advance level or end the game
                        if (_levelManager.levelIndex != 4)
                        {
                            _levelManager.LoadNextLevel();
                            Time.timeScale = 1f;
                            blocksRaycasts = false;
                            _fade.FadeIn(() =>
                            {
                                Disable();
                                blocksRaycasts = true;
                            });
                            break;
                        }
                        else
                        {
                            Trigger("closing");
                            break;
                        }
                }
            }
            else if (_action == "opening")
            {
                switch (_counter)
                {
                    case 1:
                        titleText.SetText("Controls");
                        bodyText.enabled = false;
                        controlsText.enabled = true;
                        break;
                    case 2:
                        _levelManager.InitializeNextLevel();
                        Time.timeScale = 1f;
                        blocksRaycasts = false;
                        FadeOut(() =>
                        {
                            Disable();
                            blocksRaycasts = true;
                            bodyText.enabled = true;
                            controlsText.enabled = false;
                        });
                        break;
                }
            }
            else
            {
                DismissMenuFull();
            }
        }
        
        private void DismissMenuFull()
        {
            if (_typeCoroutine != null)
                StopCoroutine(_typeCoroutine);
            switch (_action)
            {
                case "death":
                    _fade.FadeIn(() =>
                    {
                        _levelManager.LoadLevel(0);
                    });
                    break;
                case "closing":
                    Time.timeScale = 1f;
                    _fade.FadeIn(() =>
                    {
                        _levelManager.LoadLevel(0);
                    });
                    break;
                case "Minigame_Coin":
                case "Minigame_Trash":
                case "Minigame_Grandma":
                    _minigameManager.InitializeMinigame();
                    blocksRaycasts = false;
                    FadeOut(() => { 
                        Disable();
                        blocksRaycasts = true;
                    });
                    break;
            }
        }
    }
}

