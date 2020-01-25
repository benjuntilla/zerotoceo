using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    private SaveManager _saveManager;
    private UIManager _uiManager;
    private IMinigameManager _minigameManager;
    private LevelManager _levelManager;

    public readonly Dictionary<string, int> minigamePoints = new Dictionary<string, int>()
    {
        {"Minigame_Grandma_Easy", 50},
        {"Minigame_Grandma_Medium", 75},
        {"Minigame_Grandma_Hard", 100},
        {"Minigame_Trash_Easy", 50},
        {"Minigame_Trash_Medium", 75},
        {"Minigame_Trash_Hard", 100},
        {"Minigame_Coin_Easy", 50},
        {"Minigame_Coin_Medium", 75},
        {"Minigame_Coin_Hard", 100},
    };
    public static string minigameId = ""; // Full id of minigame e.g., Minigame_Grandma_Easy
    public static string minigameName = ""; // First two parts of the minigame id e.g., Minigame_Grandma
    public static string minigameDifficulty = ""; // Just the last part of the minigame id containing the difficulty
    public static Status minigameStatus = Status.None;
    public Difficulty defaultDifficulty = Difficulty.Easy; // Used for debugging
    public int minigameProgression; // Count of minigames passed on a level by level basis

    public enum Status
    {
        None,
        Pending,
        InProgress,
        Failed,
        Passed,
    }
    public enum Difficulty
    {
        Easy, 
        Medium, 
        Hard
    };

    void Awake()
    {
        _levelManager = GetComponent<LevelManager>();
        _saveManager = gameObject.GetComponent<SaveManager>();
        _minigameManager = gameObject.GetComponent<IMinigameManager>();
        _uiManager = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        _uiManager.exitEvent.AddListener(delegate { minigameStatus = Status.None; });
    }

    public string ResolveEmptyMinigame() // For debug purposes
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 5:
                minigameId = $"Minigame_Grandma_{defaultDifficulty.ToString()}";
                break;
            case 6:
                minigameId = $"Minigame_Trash_{defaultDifficulty.ToString()}";
                break;
            case 7:
                minigameId = $"Minigame_Coin_{defaultDifficulty.ToString()}";
                break;
        }
        InitializeMinigame();
        return minigameName;
    }

    public void InitializeMinigame()
    {
        minigameStatus = Status.InProgress;
        var splitName = minigameId.Split('_');
        minigameName = $"{splitName[0]}_{splitName[1]}";
        minigameDifficulty = splitName[2];
        _saveManager.Save();
    }

    public void StartMinigame()
    {
        if (_minigameManager.countDownNecessary)
            StartCoroutine(BeginCountdown());
        else
            _minigameManager.StartGame();
    }

    private IEnumerator BeginCountdown()
    {
        // TODO: Trigger UI event
        yield return new WaitForSeconds(3);
        _minigameManager.StartGame();
    }

    public void Pass()
    {
        LevelManager.nextLevelFlag = false;
        minigameStatus = Status.Passed;
        _uiManager.TriggerMinigameEndMenu(true);
    }

    public void Fail()
    {
        LevelManager.nextLevelFlag = false;
        minigameStatus = Status.Failed;
        _uiManager.TriggerMinigameEndMenu(false);
    }

    /// <summary>
    ///  Manipulates player attributes based on whether a minigame was passed or failed
    /// </summary>
    private void CheckPassOrFail()
    {
        switch (minigameStatus)
        {
            default:
                LevelManager.nextLevelFlag = false;
                minigameStatus = Status.None;
                PlayerController.Lives -= 1;
                if (PlayerController.Lives != 0)
                    _saveManager.Save();
                break;
            case Status.Passed:
                LevelManager.nextLevelFlag = false;
                minigameStatus = Status.None;
                PlayerController.Points += minigamePoints[minigameId];
                _levelManager.scoreboard["minigameBonus"] += minigamePoints[minigameId];
                minigameId = ""; // Clears the variable so the minigame cannot be replayed
                minigameProgression++;
                _saveManager.Save();
                break;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 5) return;
        CheckPassOrFail();
    }
}
