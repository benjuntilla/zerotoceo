using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static GameObject _ui, _pauseMenuUI, _hudUI, _modalUI, _modalLayout, _levelEndMenuUI, _levelEndMenuLayout, _levelEndMenuImages, _popupUI, _heartOne, _heartTwo, _heartThree, _futureToken, _businessToken, _leaderToken, _americaToken, _hudLives, _hudTokens, _saveAlert, _minigameEndMenuUI, _menuFullUI;
    private static TextMeshProUGUI _hudPointsText, _modalText, _levelEndMenuText, _levelEndMenuTitle, _popupText, _menuFullTitle, _menuFullText, _menuFullButtonText;
    private static Animator _fadeAnimator, _popupAnimator, _menuFullAnimator;
    private static CanvasGroup _menuFullCanvasGroup;
    private static string _modalAction, _menuFullAction;
    private static bool _debug;
    private static int _levelEndMenuCounter;
    private static UIManager _instance; // This allows non-static methods (e.g. coroutines) to be called in static methods via an instance of this class

    public GUIStyle debugStyle;
    public UnityEvent uiReadyEvent = new UnityEvent();
    [TextArea(3, 10)]
    public string closingMenuText, openingMenuText, gameOverText, grandmaMinigameText, coinMinigameText, trashMinigameText;
    
    void OnGUI()
    {
        if (!_debug || SceneManager.GetActiveScene().buildIndex == 0 || SceneManager.GetActiveScene().buildIndex > 4) return;
        var log =
            $"X Velocity: {GameObject.FindWithTag("Player").GetComponent<PlayerController>().velocity.x}\n" +
            $"Y Velocity: {GameObject.FindWithTag("Player").GetComponent<PlayerController>().velocity.y}\n" +
            $"Points: {PlayerController.Points}\n" +
            $"Lives: {PlayerController.Lives}\n" +
            $"Level: {LevelManager.Level}\n" +
            $"Timescale: {Time.timeScale}\n";
            GUI.Label(new Rect(10, 100, 999, 999), log, debugStyle); // Rectangle dimensions are as follows: (distance from left edge, distance from top edge, width, height)
    }
    
    void Awake()
    {
        try
        {
            _ui = GameObject.FindWithTag("UI");
        }
        catch (Exception e)
        {
            Debug.Log($"Couldn't find UI object. Exception: {e}");
        }
        try
        {
            _modalUI = _ui.transform.Find("Modal").gameObject;
            _modalLayout = _modalUI.transform.Find("Layout").gameObject;
            _modalText = _modalLayout.transform.Find("Text").gameObject.GetComponentInChildren<TextMeshProUGUI>();
            _fadeAnimator = _ui.transform.Find("Fade").gameObject.GetComponent<Animator>();
            
            _modalUI.SetActive(false);
        }
        catch (Exception e)
        {
            Debug.Log($"Couldn't find modal and/or fade objects. Exception: {e}");
        }

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            try
            {
                _menuFullUI = _ui.transform.Find("Menu Full").gameObject;
                _menuFullAnimator = _menuFullUI.GetComponent<Animator>();
                _menuFullCanvasGroup = _menuFullUI.GetComponent<CanvasGroup>();
                _menuFullTitle = _menuFullUI.transform.Find("Title").GetComponent<TextMeshProUGUI>();
                _menuFullText = _menuFullUI.transform.Find("Text").GetComponent<TextMeshProUGUI>();
                _menuFullButtonText = _menuFullUI.transform.Find("Button").GetComponentInChildren<TextMeshProUGUI>();
                _pauseMenuUI = _ui.transform.Find("Pause Menu").gameObject;
                _levelEndMenuUI = _ui.transform.Find("Level End Menu").gameObject;
                _minigameEndMenuUI = _ui.transform.Find("Minigame End Menu").gameObject;
                _levelEndMenuLayout = _levelEndMenuUI.transform.Find("Layout").gameObject;
                _levelEndMenuTitle = _levelEndMenuLayout.transform.Find("Title").gameObject.GetComponentInChildren<TextMeshProUGUI>();
                _levelEndMenuText = _levelEndMenuLayout.transform.Find("Text").gameObject.GetComponentInChildren<TextMeshProUGUI>();
                _levelEndMenuImages = _levelEndMenuLayout.transform.Find("Images").gameObject;
                _popupUI = _ui.transform.Find("Popup").gameObject;
                _popupAnimator = _popupUI.GetComponent<Animator>();
                _popupText = _ui.transform.Find("Popup").gameObject.GetComponentInChildren<TextMeshProUGUI>();
                _hudUI = _ui.transform.Find("HUD").gameObject;
                _hudPointsText = _hudUI.transform.Find("Points").gameObject.GetComponentInChildren<TextMeshProUGUI>();
                _hudLives = _hudUI.transform.Find("Lives").gameObject;
                _hudTokens = _hudUI.transform.Find("Tokens").gameObject;
                _heartOne = _hudLives.transform.Find("Heart 1").gameObject;
                _heartTwo = _hudLives.transform.Find("Heart 2").gameObject;
                _heartThree = _hudLives.transform.Find("Heart 3").gameObject;
                _futureToken = _hudTokens.transform.Find("Future").gameObject;
                _businessToken = _hudTokens.transform.Find("Business").gameObject;
                _leaderToken = _hudTokens.transform.Find("Leader").gameObject;
                _americaToken = _hudTokens.transform.Find("America").gameObject;
                _saveAlert = _hudUI.transform.Find("Save Alert").gameObject;
            
                _popupUI.SetActive(false);
                _minigameEndMenuUI.SetActive(false);
                _levelEndMenuUI.SetActive(false);
                _pauseMenuUI.SetActive(false);
                _saveAlert.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.Log($"Couldn't find other menu objects. Exception: {e}");
            }
        }

        _instance = this;
        TriggerApplicableMenus();
        uiReadyEvent.Invoke();
    }

    private void TriggerApplicableMenus()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1 && !SaveManager.LoadFlag)
            TriggerMenuFull("opening");
        else if (SceneManager.GetActiveScene().buildIndex >= 5 && MinigameManager.Minigame != "")
            TriggerMenuFull(MinigameManager.MinigameName);
        else if (SceneManager.GetActiveScene().buildIndex >= 5 && MinigameManager.Minigame == "")
            TriggerMenuFull(MinigameManager.ResolveEmptyMinigame()); 
    }

    private static void PauseGame ()
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

    private static void PlayIn(Animator animator)
    {
        animator.Play( Animator.StringToHash( "In" ) );
    }    
    
    private static void PlayOut(Animator animator)
    {
        animator.Play( Animator.StringToHash( "Out" ) );
    }

    private static IEnumerator PlayInThenAction(Animator animator, Action action)
    {
        PlayIn(animator);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("In")) // Wait until the animation begins playing
            yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("In")) // Wait until the animation transitions to another
            yield return null;
        action();
    }
    
    private static IEnumerator PlayOutThenAction(Animator animator, Action action)
    {
        PlayOut(animator);
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Out")) // Wait until the animation begins playing
            yield return null;
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Out")) // Wait until the animation transitions to another
            yield return null;
        action();
    }

    private static void FadeInAndLoadLevelIndex(int index)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(PlayInThenAction(_fadeAnimator, () => { LevelManager.LoadLevelIndex(index); }));
    }

    public static void FadeInAndLoadLevelName(string levelName)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(PlayInThenAction(_fadeAnimator, () => { LevelManager.LoadLevelName(levelName); }));
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
                MinigameManager.InitializeMinigame();
                FadeInAndLoadLevelName(MinigameManager.MinigameName);
                break;
            case "save":
                ResumeGame();
                _modalUI.SetActive(false);
                SaveManager.Save();
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
        _levelEndMenuText.SetText($"Good job completing level {LevelManager.Level}!");
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
                if (LevelManager.Level == 4) // Skip the clothing slide on level 4 since it doesnt exist
                    _levelEndMenuCounter++;
                break;
            case 2: // Show new clothing
                _levelEndMenuTitle.SetText("Got new clothing!");
                _levelEndMenuText.SetText("\n\n\n\n\n\n\n");
                switch (LevelManager.Level)
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
                switch (LevelManager.Level)
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
                if (LevelManager.Level != 4)
                {
                    Time.timeScale = 1f;
                    StopAllCoroutines();
                    StartCoroutine(PlayInThenAction(_fadeAnimator, () =>
                    {
                        _levelEndMenuUI.SetActive(false);
                        LevelManager.NextLevelFlag = true;
                        LevelManager.Level++;
                        LevelManager.LoadLevelIndex(LevelManager.Level);
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

    public static void TriggerPopup(string id)
    {
        _popupUI.SetActive(true);
        switch (id)
        {
            case "minigame":
                _popupText.SetText("A minigame is now playable! Go to the exit door to start.");
                _instance.StartCoroutine(PopupInThenOut(5));
                break;
            case "save":
                _popupText.SetText("Saved game.");
                _instance.StartCoroutine(PopupInThenOut(2));
                break;
        }
    }

    private static IEnumerator PopupInThenOut(int seconds)
    {
        PlayIn(_popupAnimator);
        yield return new WaitForSeconds(seconds);
        _instance.StartCoroutine(PlayOutThenAction(_popupAnimator, () => _popupUI.SetActive(false)));
    }
    
    private void TriggerMenuFull(string id)
    {
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
                Time.timeScale = 1f;
                StartCoroutine(PlayOutThenAction(_menuFullAnimator, () => _menuFullUI.SetActive(false)));
                LevelManager.InitializeNextLevel();
                break;
            case "Minigame_Coin":
            case "Minigame_Trash":
            case "Minigame_Grandma":
                Time.timeScale = 1f;
                StartCoroutine(PlayOutThenAction(_menuFullAnimator, () => _menuFullUI.SetActive(false)));
                break;
        }
    }
    
    public static void TriggerMinigameEndMenu(bool pass)
    {
        _minigameEndMenuUI.GetComponentInChildren<TextMeshProUGUI>().SetText(pass ? $"You passed the minigame. (Gained {MinigameManager.MinigamePoints[MinigameManager.Minigame]} points)" : "You failed the minigame. (Lost 1 life)");
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
        if (_pauseMenuUI && Input.GetKeyDown(KeyCode.Escape))
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

        # region Update HUD
        if (!_hudUI) return;
        
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
        switch (LevelManager.Level)
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
        #endregion
    }
}
