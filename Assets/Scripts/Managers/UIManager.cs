using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private string _modalAction, _menuFullAction;
    private bool _debug, _triggeredLevelUpPopup, _triggeredDeathMenu, _popupReady = true;
    private int _levelEndMenuCounter, _menuFullCounter;
    private MinigameManager _minigameManager;
    private SaveManager _saveManager;
    private LevelManager _levelManager;
    private Queue<string> _popupQueue = new Queue<string>();

    public GameObject pauseMenuUI, hudUI, modalUI, levelEndMenuUI, levelEndMenuImages, popupUI, heartOne, heartTwo, heartThree, futureToken, businessToken, leaderToken, americaToken, hudLives, hudTokens, minigameEndMenuUI, menuFullUI, mainMenuUI;
    public TextMeshProUGUI hudPointsText, modalText, levelEndMenuText, levelEndMenuTitle, popupText, menuFullTitle, menuFullText, menuFullButtonText, menuFullControlsText, minigameEndMenuText;
    public Animator fadeAnimator, popupAnimator, menuFullAnimator;
    public CanvasGroup menuFullCanvasGroup;
    public GUIStyle debugStyle;
    public UnityEvent uiReadyEvent = new UnityEvent(), exitEvent = new UnityEvent();
    [TextArea(3, 10)]
    public string closingMenuText, openingMenuText, gameOverText, grandmaMinigameText, coinMinigameText, trashMinigameText;

    void Awake()
    {
        if (!GameObject.FindWithTag("GameManagers")) return;
        _saveManager = FindObjectOfType<SaveManager>();
        _levelManager = FindObjectOfType<LevelManager>();
        _minigameManager = FindObjectOfType<MinigameManager>();
        if (SceneManager.GetActiveScene().buildIndex >= 5)
            _minigameManager = GameObject.FindWithTag("GameManagers").GetComponent<MinigameManager>();

        TriggerApplicableMenus();
        uiReadyEvent.Invoke();
    }
    
    void OnGUI()
    {
        if (!_debug ) return;
        var log = "";
        switch (_levelManager.currentLevelType)
        {
            case LevelManager.LevelType.Level:
                log =
                    $"X Velocity: {GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity.x}\n" +
                    $"Y Velocity: {GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>().velocity.y}\n" +
                    $"Points: {PlayerController.points}\n" +
                    $"Lives: {PlayerController.lives}\n" +
                    $"Level: {_levelManager.levelIndex}\n" +
                    $"Timescale: {Time.timeScale}\n";
                GUI.Label(new Rect(10, 100, 999, 999), log, debugStyle); // Rectangle dimensions are as follows: (distance from left edge, distance from top edge, width, height)
                break;
            case LevelManager.LevelType.Minigame:
                log =
                    $"Minigame ID: {MinigameManager.minigameId}\n" +
                    $"Minigame Status: {MinigameManager.minigameStatus}\n" +
                    $"Timescale: {Time.timeScale}\n";           
                GUI.Label(new Rect(10, 10, 999, 999), log, debugStyle); // Rectangle dimensions are as follows: (distance from left edge, distance from top edge, width, height)
                break;
        }
    }

    private void TriggerApplicableMenus()
    {
        mainMenuUI.SetActive(SceneManager.GetActiveScene().buildIndex == 0);
        if (SceneManager.GetActiveScene().buildIndex == 1 && !SaveManager.loadFlag)
            TriggerMenuFull("opening");
        else if (SceneManager.GetActiveScene().buildIndex >= 5 && MinigameManager.minigameId != "")
            TriggerMenuFull(MinigameManager.minigameName);
        else if (SceneManager.GetActiveScene().buildIndex >= 5 && MinigameManager.minigameId == "")
            TriggerMenuFull(_minigameManager.ResolveEmptyMinigame()); 
    }

    private void PauseGame ()
    {
        if (pauseMenuUI)
            pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame ()
    {
        if (pauseMenuUI)
            pauseMenuUI.SetActive(false);
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

    private void FadeInAndLoadNewLevel(object param)
    {
        StopAllCoroutines();
        StartCoroutine(PlayInThenAction(fadeAnimator, () => { _saveManager.LoadNewLevel(param); }));
    }

    private void FadeInAndLoadSavedLevel()
    {
        StopAllCoroutines();
        StartCoroutine(PlayInThenAction(fadeAnimator, () => { _saveManager.LoadSavedLevel(); }));
    }

    public void TriggerModal (string id)
    {
        _modalAction = id;
        switch (id)
        {
            case "minigame":
                modalText.SetText("Do you want to start the minigame?");
                modalUI.SetActive(true);
                break;
            case "save":
                modalText.SetText("Are you sure you want to save? This will overwrite current save data!");
                modalUI.SetActive(true);
                break;
            case "load":
                modalText.SetText("Load save data?");
                modalUI.SetActive(true);
                break;
            case "loadFromMain":
                modalText.SetText("Load save data?");
                modalUI.SetActive(true);
                break;
            case "new":
                modalText.SetText("Are you sure you want to start a new game? This will NOT load previous save data!");
                modalUI.SetActive(true);
                break;
            case "quit":
                modalText.SetText("Are you sure you want to quit? This will discard unsaved game data!");
                modalUI.SetActive(true);
                break;
            case "main":
                modalText.SetText("Are you sure you want to exit to the main menu? This will discard unsaved game data!");
                modalUI.SetActive(true);
                break;
        }
    }

    public void ConfirmModal()
    {
        switch (_modalAction)
        {
            case "minigame":
                modalUI.SetActive(false);
                hudUI.SetActive(false);
                _minigameManager.InitializeMinigame();
                FadeInAndLoadNewLevel(MinigameManager.minigameName);
                break;
            case "save":
                ResumeGame();
                modalUI.SetActive(false);
                _saveManager.Save();
                break;
            case "load":
                ResumeGame();
                modalUI.SetActive(false);
                _saveManager.Load();
                break;
            case "loadFromMain":
                ResumeGame();
                modalUI.SetActive(false);
                FadeInAndLoadSavedLevel();
                break;
            case "new":
                ResumeGame();
                modalUI.SetActive(false);
                FadeInAndLoadNewLevel(1);
                break;
            case "main":
                ResumeGame();
                exitEvent.Invoke();
                modalUI.SetActive(false);
                FadeInAndLoadNewLevel(0);
                break;
            case "quit":
                modalUI.SetActive(false);
                Application.Quit();
                break;
        }
    }

    public void DismissModal()
    {
        modalUI.SetActive(false);
    }

    public void TriggerLevelEndMenu()
    {
        Time.timeScale = 0f;
        _levelEndMenuCounter = 0;
        levelEndMenuTitle.SetText("Level Up!");
        levelEndMenuText.SetText($"Good job completing level {_levelManager.levelIndex}!");
        levelEndMenuUI.SetActive(true);    
    }

    public void ContinueLevelEndMenu()
    {
        foreach (Transform child in levelEndMenuImages.transform)
            child.gameObject.SetActive(false);
        _levelEndMenuCounter++;
        switch (_levelEndMenuCounter)
        {
            case 1: // Scoreboard page
                levelEndMenuTitle.SetText("Scoreboard");
                levelEndMenuText.SetText(
                    $"Dialogue bonus: {_levelManager.scoreboard["dialogueBonus"]}\n" +
                    $"Dialogue penalty: {_levelManager.scoreboard["dialoguePenalty"]}\n" +
                    $"Minigame bonus: {_levelManager.scoreboard["minigameBonus"]}\n" +
                    $"Total points: {PlayerController.points}\n"
                );
                if (_levelManager.levelIndex == 4) // Skip the clothing slide on level 4 since it doesnt exist
                    _levelEndMenuCounter++;
                break;
            case 2: // Show new clothing
                levelEndMenuTitle.SetText("Got new clothing!");
                levelEndMenuText.SetText("\n\n\n\n\n\n\n");
                switch (_levelManager.levelIndex)
                {
                    case 1: // Level has not actually increased yet, so the desired value minus one must be used.
                        levelEndMenuImages.transform.Find("Player 2").gameObject.SetActive(true);
                        break;
                    case 2:
                        levelEndMenuImages.transform.Find("Player 3").gameObject.SetActive(true);
                        break;                    
                    case 3:
                        levelEndMenuImages.transform.Find("Player 4").gameObject.SetActive(true);
                        break;
                }
                break;
            case 3: // Show token
                levelEndMenuTitle.SetText("Received a BAA token!");
                levelEndMenuText.SetText("\n\n\n\n\n\n\n");
                switch (_levelManager.levelIndex)
                {
                    case 1: // Level has not actually increased yet, so the desired value minus one must be used.
                        levelEndMenuImages.transform.Find("Future").gameObject.SetActive(true);
                        break;
                    case 2:
                        levelEndMenuImages.transform.Find("Business").gameObject.SetActive(true);
                        break;                    
                    case 3:
                        levelEndMenuImages.transform.Find("Leader").gameObject.SetActive(true);
                        break;
                    case 4:
                        levelEndMenuImages.transform.Find("America").gameObject.SetActive(true);
                        break;
                }
                break;
            case 4: // Either advance level or end the game
                if (_levelManager.levelIndex != 4)
                {
                    Time.timeScale = 1f;
                    StopAllCoroutines();
                    StartCoroutine(PlayInThenAction(fadeAnimator, () =>
                    {
                        levelEndMenuUI.SetActive(false);
                        _levelManager.LoadNextLevel();
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

    public void QueuePopup(string id)
    {
        _popupQueue.Enqueue(id);
    }

    private void TriggerPopup()
    {
        switch (_popupQueue.Dequeue())
        {
            case "minigame":
                popupText.SetText("A minigame is now playable! Go to the exit door to start.");
                StartCoroutine(PopupInThenOut(5));
                break;
            case "save":
                popupText.SetText("Saved game.");
                StartCoroutine(PopupInThenOut(2));
                break;
            case "levelup":
                popupText.SetText("You can now level up! Go to the level door to advance.");
                StartCoroutine(PopupInThenOut(5));
                break;
        }
    }

    private IEnumerator PopupInThenOut(int seconds)
    {
        _popupReady = false;
        popupUI.SetActive(true);
        PlayIn(popupAnimator);
        yield return new WaitForSeconds(seconds);
        StartCoroutine(PlayOutThenAction(popupAnimator, () =>
        {
            popupUI.SetActive(false);
            _popupReady = true;
        }));
    }
    
    private void TriggerMenuFull(string id)
    {
        _menuFullCounter = 0;
        menuFullCanvasGroup.blocksRaycasts = true;
        menuFullUI.SetActive(true);
        _menuFullAction = id;
        switch (id)
        {
            case "opening":
                menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayOutThenAction(fadeAnimator, () =>
                {
                    menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));
                menuFullTitle.SetText("The beginning");
                StartCoroutine(TypeText(menuFullText, openingMenuText));
                menuFullButtonText.SetText("Continue");
                break;
            case "death":
                menuFullTitle.SetText("Game Over");
                StartCoroutine(TypeText(menuFullText, gameOverText));
                menuFullButtonText.SetText("Main Menu");
                break;
            case "closing":
                Time.timeScale = 1f;
                menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayInThenAction(menuFullAnimator, () =>
                {
                    menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));                
                menuFullTitle.SetText("You Beat the Game!");
                StartCoroutine(TypeText(menuFullText, closingMenuText));
                menuFullButtonText.SetText("Main Menu");
                break;
            case "Minigame_Grandma":
                menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayOutThenAction(fadeAnimator, () =>
                {
                    menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));                
                menuFullTitle.SetText("Minigame: Cross the street");
                StartCoroutine(TypeText(menuFullText, grandmaMinigameText));
                menuFullButtonText.SetText("Continue");
                break;
            case "Minigame_Trash":
                menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayOutThenAction(fadeAnimator, () =>
                {
                    menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));                
                menuFullTitle.SetText("Minigame: Collect the trash");
                StartCoroutine(TypeText(menuFullText, trashMinigameText));
                menuFullButtonText.SetText("Continue");
                break;
            case "Minigame_Coin":
                menuFullCanvasGroup.blocksRaycasts = false;
                StartCoroutine(PlayOutThenAction(fadeAnimator, () =>
                {
                    menuFullCanvasGroup.blocksRaycasts = true;
                    Time.timeScale = 0f;
                }));                
                menuFullTitle.SetText("Minigame: Sort the coins");
                StartCoroutine(TypeText(menuFullText, coinMinigameText));
                menuFullButtonText.SetText("Continue");
                break;
        }
    }

    public void DismissMenuFull()
    {
        StopCoroutine("TypeText");
        menuFullCanvasGroup.blocksRaycasts = false;
        switch (_menuFullAction)
        {
            case "death":
                FadeInAndLoadNewLevel(0);
                break;
            case "closing":
                Time.timeScale = 1f;
                FadeInAndLoadNewLevel(0);
                break;
            case "opening":
                switch (_menuFullCounter)
                {
                    case 0:
                        menuFullCanvasGroup.blocksRaycasts = true;
                        menuFullTitle.SetText("Controls");
                        menuFullText.enabled = false;
                        menuFullControlsText.enabled = true;
                        break;
                    case 1:
                        Time.timeScale = 1f;
                        StartCoroutine(PlayOutThenAction(menuFullAnimator, () =>
                        {
                            menuFullUI.SetActive(false);
                            menuFullText.enabled = true;
                            menuFullControlsText.enabled = false;
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
                StartCoroutine(PlayOutThenAction(menuFullAnimator, () =>
                {
                    menuFullUI.SetActive(false);
                    _minigameManager.StartMinigame();
                }));
                break;
        }
    }
    
    public void TriggerMinigameEndMenu(bool pass)
    {
        minigameEndMenuText.SetText(pass ? $"You passed the minigame. (Gained {_minigameManager.minigamePoints[MinigameManager.minigameId]} points)" : "You failed the minigame. (Lost 1 life)");
        minigameEndMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void DismissMinigameEndMenu()
    {
        Time.timeScale = 1f;
        minigameEndMenuUI.SetActive(false);
        StartCoroutine(PlayInThenAction(fadeAnimator, () =>
            {
                GameObject.FindWithTag("UI").transform.Find("HUD").gameObject.SetActive(true);
                _saveManager.LoadSavedLevel();
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
        // Detect when to trigger a popup in queue
        if (_popupQueue.Count > 0 && _popupReady)
            TriggerPopup();
        
        // Detect when to pause game
        if (pauseMenuUI && Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
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
        if (_levelManager.currentLevelType == LevelManager.LevelType.Level && PlayerController.points >= _levelManager.nextLevelRequirements[_levelManager.levelIndex] && !_triggeredLevelUpPopup)
        {
            _triggeredLevelUpPopup = true;
            QueuePopup("levelup");
        }
        
        # region Update HUD
        if (hudUI)
        {
            hudPointsText.SetText($"Points: {PlayerController.points}");
        
            foreach (Transform child in hudLives.transform)
                child.gameObject.SetActive(false);
            switch (PlayerController.lives)
            {
                case 0:
                    if (!_triggeredDeathMenu)
                    {
                        _triggeredDeathMenu = true;
                        TriggerMenuFull("death");
                    }
                    break;
                case 1:
                    heartOne.SetActive(true);
                    break;
                case 2:
                    heartOne.SetActive(true);
                    heartTwo.SetActive(true);
                    break;
                default:
                    heartOne.SetActive(true);
                    heartTwo.SetActive(true);
                    heartThree.SetActive(true);
                    break;
            }
        
            foreach (Transform child in hudTokens.transform)
                child.gameObject.SetActive(false);
            switch (_levelManager.levelIndex)
            {
                case 2:
                    futureToken.SetActive(true);
                    break;
                case 3:
                    futureToken.SetActive(true);
                    businessToken.SetActive(true);
                    break;
                case 4:
                    futureToken.SetActive(true);
                    businessToken.SetActive(true);
                    leaderToken.SetActive(true);
                    break;
                case 5:
                    futureToken.SetActive(true);
                    businessToken.SetActive(true);
                    leaderToken.SetActive(true);
                    americaToken.SetActive(true);
                    break;
            }
        }
        #endregion
    }
}
