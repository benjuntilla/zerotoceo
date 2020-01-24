using System;
using System.Collections;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static string _modalAction, _menuFullAction;
    private static bool _debug, _triggeredLevelUpPopup;
    private static int _levelEndMenuCounter, _menuFullCounter;
    private MinigameManager _minigameManager;
    private SaveManager _saveManager;
    private LevelManager _levelManager;

    public GameObject _pauseMenuUI, _hudUI, _modalUI, _levelEndMenuUI, _levelEndMenuImages, _popupUI, _heartOne, _heartTwo, _heartThree, _futureToken, _businessToken, _leaderToken, _americaToken, _hudLives, _hudTokens, _minigameEndMenuUI, _menuFullUI, _mainMenuUI;
    public TextMeshProUGUI _hudPointsText, _modalText, _levelEndMenuText, _levelEndMenuTitle, _popupText, _menuFullTitle, _menuFullText, _menuFullButtonText, _menuFullControlsText;
    public Animator _fadeAnimator, _popupAnimator, _menuFullAnimator;
    public CanvasGroup _menuFullCanvasGroup;
    public GUIStyle debugStyle;
    public UnityEvent uiReadyEvent = new UnityEvent(), exitEvent = new UnityEvent();
    [TextArea(3, 10)]
    public string closingMenuText, openingMenuText, gameOverText, grandmaMinigameText, coinMinigameText, trashMinigameText;

    void Awake()
    {
        var gameManagers = GameObject.FindWithTag("GameManagers");
        _saveManager = gameManagers.GetComponent<SaveManager>();
        _levelManager = gameManagers.GetComponent<LevelManager>();
        if (SceneManager.GetActiveScene().buildIndex >= 5)
            _minigameManager = GameObject.FindWithTag("GameManagers").GetComponent<MinigameManager>();

        TriggerApplicableMenus();
        uiReadyEvent.Invoke();
    }
    
    void OnGUI()
    {
        if (!_debug ) return;
        var log = "";
        switch (LevelManager.CurrentLevelType)
        {
            case LevelManager.LevelType.Level:
                log =
                    $"X Velocity: {GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity.x}\n" +
                    $"Y Velocity: {GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity.y}\n" +
                    $"Points: {PlayerController.Points}\n" +
                    $"Lives: {PlayerController.Lives}\n" +
                    $"Level: {LevelManager.LevelIndex}\n" +
                    $"Timescale: {Time.timeScale}\n";
                GUI.Label(new Rect(10, 100, 999, 999), log, debugStyle); // Rectangle dimensions are as follows: (distance from left edge, distance from top edge, width, height)
                break;
            case LevelManager.LevelType.Minigame:
                log =
                    $"Minigame ID: {MinigameManager.MinigameID}\n" +
                    $"Minigame Status: {MinigameManager.MinigameStatus}\n" +
                    $"Timescale: {Time.timeScale}\n";           
                GUI.Label(new Rect(10, 10, 999, 999), log, debugStyle); // Rectangle dimensions are as follows: (distance from left edge, distance from top edge, width, height)
                break;
        }
    }

    private void TriggerApplicableMenus()
    {
        _mainMenuUI.SetActive(SceneManager.GetActiveScene().buildIndex == 0);
        if (SceneManager.GetActiveScene().buildIndex == 1 && !SaveManager.LoadFlag)
            TriggerMenuFull("opening");
        else if (SceneManager.GetActiveScene().buildIndex >= 5 && MinigameManager.MinigameID != "")
            TriggerMenuFull(MinigameManager.MinigameName);
        else if (SceneManager.GetActiveScene().buildIndex >= 5 && MinigameManager.MinigameID == "")
            TriggerMenuFull(_minigameManager.ResolveEmptyMinigame()); 
    }

    private void PauseGame ()
    {
        if (_pauseMenuUI)
            _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame ()
    {
        if (_pauseMenuUI)
            _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void PlayIn(Animator animator)
    {
        animator.Play( Animator.StringToHash( "In" ) );
    }    
    
    private void PlayOut(Animator animator)
    {
        animator.Play( Animator.StringToHash( "Out" ) );
    }

    private IEnumerator PlayInThenAction(Animator animator, Action action)
    {
        PlayIn(animator);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("In")) // Wait until the animation begins playing
            yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("In")) // Wait until the animation transitions to another
            yield return null;
        action();
    }
    
    private IEnumerator PlayOutThenAction(Animator animator, Action action)
    {
        PlayOut(animator);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Out")) // Wait until the animation begins playing
            yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Out")) // Wait until the animation transitions to another
            yield return null;
        action();
    }

    private void FadeInAndLoadLevelIndex(int index)
    {
        StopAllCoroutines();
        StartCoroutine(PlayInThenAction(_fadeAnimator, () => { LevelManager.LoadLevelIndex(index); }));
    }

    public void FadeInAndLoadLevelName(string levelName)
    {
        StopAllCoroutines();
        StartCoroutine(PlayInThenAction(_fadeAnimator, () => { LevelManager.LoadLevelName(levelName); }));
    }

    public void TriggerModal (string id)
    {
        _modalAction = id;
        switch (id)
        {
            case "minigame":
                _modalText.SetText("Do you want to start the minigame?");
                _modalUI.SetActive(true);
                break;
            case "save":
                _modalText.SetText("Are you sure you want to save? This will overwrite current save data!");
                _modalUI.SetActive(true);
                break;
            case "load":
                _modalText.SetText("Load save data?");
                _modalUI.SetActive(true);
                break;
            case "loadFromMain":
                _modalText.SetText("Load save data?");
                _modalUI.SetActive(true);
                break;
            case "new":
                _modalText.SetText("Are you sure you want to start a new game? This will NOT load previous save data!");
                _modalUI.SetActive(true);
                break;
            case "quit":
                _modalText.SetText("Are you sure you want to quit? This will discard unsaved game data!");
                _modalUI.SetActive(true);
                break;
            case "main":
                _modalText.SetText("Are you sure you want to exit to the main menu? This will discard unsaved game data!");
                _modalUI.SetActive(true);
                break;
        }
    }

    public void ConfirmModal()
    {
        switch (_modalAction)
        {
            case "minigame":
                _modalUI.SetActive(false);
                _hudUI.SetActive(false);
                _minigameManager.InitializeMinigame();
                FadeInAndLoadLevelName(MinigameManager.MinigameName);
                break;
            case "save":
                ResumeGame();
                _modalUI.SetActive(false);
                _saveManager.Save();
                break;
            case "load":
                ResumeGame();
                _modalUI.SetActive(false);
                SaveManager.Load();
                break;
            case "loadFromMain":
                ResumeGame();
                _modalUI.SetActive(false);
                SaveManager.LoadFlag = true;
                FadeInAndLoadLevelIndex(SaveManager.GetSavedLevelIndex());
                break;
            case "new":
                ResumeGame();
                _modalUI.SetActive(false);
                SaveManager.LoadFlag = false;
                FadeInAndLoadLevelIndex(1);
                break;
            case "main":
                ResumeGame();
                exitEvent.Invoke();
                _modalUI.SetActive(false);
                SaveManager.LoadFlag = false;
                FadeInAndLoadLevelIndex(0);
                break;
            case "quit":
                _modalUI.SetActive(false);
                Application.Quit();
                break;
        }
    }

    public void DismissModal()
    {
        _modalUI.SetActive(false);
    }

    public void TriggerLevelEndMenu()
    {
        Time.timeScale = 0f;
        _levelEndMenuCounter = 0;
        _levelEndMenuTitle.SetText("Level Up!");
        _levelEndMenuText.SetText($"Good job completing level {LevelManager.LevelIndex}!");
        _levelEndMenuUI.SetActive(true);    
    }

    public void ContinueLevelEndMenu()
    {
        foreach (Transform child in _levelEndMenuImages.transform)
            child.gameObject.SetActive(false);
        _levelEndMenuCounter++;
        switch (_levelEndMenuCounter)
        {
            case 1: // Scoreboard page
                _levelEndMenuTitle.SetText("Scoreboard");
                _levelEndMenuText.SetText(
                    $"Dialogue bonus: {LevelManager.Scoreboard["dialogueBonus"]}\n" +
                    $"Dialogue penalty: {LevelManager.Scoreboard["dialoguePenalty"]}\n" +
                    $"Minigame bonus: {LevelManager.Scoreboard["minigameBonus"]}\n" +
                    $"Total points: {PlayerController.Points}\n"
                );
                if (LevelManager.LevelIndex == 4) // Skip the clothing slide on level 4 since it doesnt exist
                    _levelEndMenuCounter++;
                break;
            case 2: // Show new clothing
                _levelEndMenuTitle.SetText("Got new clothing!");
                _levelEndMenuText.SetText("\n\n\n\n\n\n\n");
                switch (LevelManager.LevelIndex)
                {
                    case 1: // Level has not actually increased yet, so the desired value minus one must be used.
                        _levelEndMenuImages.transform.Find("Player 2").gameObject.SetActive(true);
                        break;
                    case 2:
                        _levelEndMenuImages.transform.Find("Player 3").gameObject.SetActive(true);
                        break;                    
                    case 3:
                        _levelEndMenuImages.transform.Find("Player 4").gameObject.SetActive(true);
                        break;
                }
                break;
            case 3: // Show token
                _levelEndMenuTitle.SetText("Received a BAA token!");
                _levelEndMenuText.SetText("\n\n\n\n\n\n\n");
                switch (LevelManager.LevelIndex)
                {
                    case 1: // Level has not actually increased yet, so the desired value minus one must be used.
                        _levelEndMenuImages.transform.Find("Future").gameObject.SetActive(true);
                        break;
                    case 2:
                        _levelEndMenuImages.transform.Find("Business").gameObject.SetActive(true);
                        break;                    
                    case 3:
                        _levelEndMenuImages.transform.Find("Leader").gameObject.SetActive(true);
                        break;
                    case 4:
                        _levelEndMenuImages.transform.Find("America").gameObject.SetActive(true);
                        break;
                }
                break;
            case 4: // Either advance level or end the game
                if (LevelManager.LevelIndex != 4)
                {
                    Time.timeScale = 1f;
                    StopAllCoroutines();
                    StartCoroutine(PlayInThenAction(_fadeAnimator, () =>
                    {
                        _levelEndMenuUI.SetActive(false);
                        LevelManager.NextLevelFlag = true;
                        LevelManager.LevelIndex++;
                        LevelManager.LoadLevelIndex(LevelManager.LevelIndex);
                    }));
                    break;
                }
                else
                {
                    TriggerMenuFull("closing");
                    break;
                }
        }
    }

    public void TriggerPopup(string id)
    {
        _popupUI.SetActive(true);
        switch (id)
        {
            case "minigame":
                _popupText.SetText("A minigame is now playable! Go to the exit door to start.");
                StartCoroutine(PopupInThenOut(5));
                break;
            case "save":
                _popupText.SetText("Saved game.");
                StartCoroutine(PopupInThenOut(2));
                break;
            case "levelup":
                _popupText.SetText("You can now level up! Go to the level door to advance.");
                StartCoroutine(PopupInThenOut(5));
                break;
        }
    }

    private IEnumerator PopupInThenOut(int seconds)
    {
        PlayIn(_popupAnimator);
        yield return new WaitForSeconds(seconds);
        StartCoroutine(PlayOutThenAction(_popupAnimator, () => _popupUI.SetActive(false)));
    }
    
    private void TriggerMenuFull(string id)
    {
        _menuFullCounter = 0;
        _menuFullCanvasGroup.blocksRaycasts = true;
        _menuFullUI.SetActive(true);
        _menuFullAction = id;
        switch (id)
        {
            case "opening":
                _menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayOutThenAction(_fadeAnimator, () =>
                {
                    _menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));
                _menuFullTitle.SetText("The beginning");
                StartCoroutine(TypeText(_menuFullText, openingMenuText));
                _menuFullButtonText.SetText("Continue");
                break;
            case "death":
                _menuFullTitle.SetText("Game Over");
                StartCoroutine(TypeText(_menuFullText, gameOverText));
                _menuFullButtonText.SetText("Main Menu");
                break;
            case "closing":
                Time.timeScale = 1f;
                _menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayInThenAction(_menuFullAnimator, () =>
                {
                    _menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));                
                _menuFullTitle.SetText("You Beat the Game!");
                StartCoroutine(TypeText(_menuFullText, closingMenuText));
                _menuFullButtonText.SetText("Main Menu");
                break;
            case "Minigame_Grandma":
                _menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayOutThenAction(_fadeAnimator, () =>
                {
                    _menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));                
                _menuFullTitle.SetText("Minigame: Cross the street");
                StartCoroutine(TypeText(_menuFullText, grandmaMinigameText));
                _menuFullButtonText.SetText("Continue");
                break;
            case "Minigame_Trash":
                _menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayOutThenAction(_fadeAnimator, () =>
                {
                    _menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));                
                _menuFullTitle.SetText("Minigame: Collect the trash");
                StartCoroutine(TypeText(_menuFullText, trashMinigameText));
                _menuFullButtonText.SetText("Continue");
                break;
            case "Minigame_Coin":
                _menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayOutThenAction(_fadeAnimator, () =>
                {
                    _menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));                
                _menuFullTitle.SetText("Minigame: Sort the coins");
                StartCoroutine(TypeText(_menuFullText, coinMinigameText));
                _menuFullButtonText.SetText("Continue");
                break;
        }
    }

    public void DismissMenuFull()
    {
        StopCoroutine("TypeText");
        _menuFullCanvasGroup.blocksRaycasts = false;
        switch (_menuFullAction)
        {
            case "death":
                SaveManager.LoadFlag = false;
                FadeInAndLoadLevelIndex(0);
                break;
            case "closing":
                Time.timeScale = 1f;
                SaveManager.LoadFlag = false;
                FadeInAndLoadLevelIndex(0);
                break;
            case "opening":
                switch (_menuFullCounter)
                {
                    case 0:
                        _menuFullCanvasGroup.blocksRaycasts = true;
                        _menuFullTitle.SetText("Controls");
                        _menuFullText.enabled = false;
                        _menuFullControlsText.enabled = true;
                        break;
                    case 1:
                        Time.timeScale = 1f;
                        StartCoroutine(PlayOutThenAction(_menuFullAnimator, () =>
                        {
                            _menuFullUI.SetActive(false);
                            _menuFullText.enabled = true;
                            _menuFullControlsText.enabled = false;
                        }));
                        _levelManager.InitializeNextLevel();
                        break;
                }
                _menuFullCounter++;
                break;
            case "Minigame_Coin":
            case "Minigame_Trash":
            case "Minigame_Grandma":
                Time.timeScale = 1f;
                StartCoroutine(PlayOutThenAction(_menuFullAnimator, () => _menuFullUI.SetActive(false)));
                break;
        }
    }
    
    public void TriggerMinigameEndMenu(bool pass)
    {
        _minigameEndMenuUI.GetComponentInChildren<TextMeshProUGUI>().SetText(pass ? $"You passed the minigame. (Gained {MinigameManager.MinigamePoints[MinigameManager.MinigameID]} points)" : "You failed the minigame. (Lost 1 life)");
        _minigameEndMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DismissMinigameEndMenu()
    {
        Time.timeScale = 1f;
        _minigameEndMenuUI.SetActive(false);
        SaveManager.LoadFlag = true;
        StartCoroutine(PlayInThenAction(_fadeAnimator, () =>
            {
                GameObject.FindWithTag("UI").transform.Find("HUD").gameObject.SetActive(true);
                LevelManager.LoadLevelIndex(SaveManager.GetSavedLevelIndex());
            }));
    }
    
    private IEnumerator TypeText(TextMeshProUGUI textObject, string text)
    {
        var t = "";
        textObject.SetText("");
        foreach (var letter in text.ToCharArray())
        {
            t += letter;
            textObject.SetText(t);
            yield return null; // Wait a frame
        }
    }

    void Update ()
    {
        // Detect when to pause game
        if (_pauseMenuUI && Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Time.timeScale == 0f)
                ResumeGame();
            else
                PauseGame();
        }
        
        // Detect when to show debug info
        if (Input.GetKeyDown(KeyCode.F3))
        {
            _debug = !_debug;
        }
        
        // Detect when to level up the player
        if (LevelManager.CurrentLevelType == LevelManager.LevelType.Level && PlayerController.Points >= LevelManager.NextLevelRequirements[LevelManager.LevelIndex] && !_triggeredLevelUpPopup)
        {
            _triggeredLevelUpPopup = true;
            TriggerPopup("levelup");
        }


        # region Update HUD
        if (_hudUI)
        {
            _hudPointsText.SetText($"Points: {PlayerController.Points}");
        
            foreach (Transform child in _hudLives.transform)
                child.gameObject.SetActive(false);
            switch (PlayerController.Lives)
            {
                case -1:
                    break;
                case 0:
                    TriggerMenuFull("death");
                    PlayerController.Lives = -1;
                    break;
                case 1:
                    _heartOne.SetActive(true);
                    break;
                case 2:
                    _heartOne.SetActive(true);
                    _heartTwo.SetActive(true);
                    break;
                default:
                    _heartOne.SetActive(true);
                    _heartTwo.SetActive(true);
                    _heartThree.SetActive(true);
                    break;
            }
        
            foreach (Transform child in _hudTokens.transform)
                child.gameObject.SetActive(false);
            switch (LevelManager.LevelIndex)
            {
                case 2:
                    _futureToken.SetActive(true);
                    break;
                case 3:
                    _futureToken.SetActive(true);
                    _businessToken.SetActive(true);
                    break;
                case 4:
                    _futureToken.SetActive(true);
                    _businessToken.SetActive(true);
                    _leaderToken.SetActive(true);
                    break;
                case 5:
                    _futureToken.SetActive(true);
                    _businessToken.SetActive(true);
                    _leaderToken.SetActive(true);
                    _americaToken.SetActive(true);
                    break;
            }
        }
        #endregion
    }
}
