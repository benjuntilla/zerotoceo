using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    private SaveManager _saveManager;
    private UIManager _uiManager;
    private IMinigameManager _minigameManager;
    private LevelManager _levelManager;

    public static readonly Dictionary<string, int> MinigamePoints = new Dictionary<string, int>()
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
    public static string MinigameID = ""; // Full id of minigame e.g., Minigame_Grandma_Easy
    public static string MinigameName = ""; // First two parts of the minigame id e.g., Minigame_Grandma
    public static string MinigameDifficulty = ""; // Just the last part of the minigame id containing the difficulty
    public static Status MinigameStatus = Status.None;
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
        _uiManager.exitEvent.AddListener(delegate { MinigameStatus = Status.None; });
    }

    public string ResolveEmptyMinigame() // For debug purposes
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 5:
                MinigameID = $"Minigame_Grandma_{defaultDifficulty.ToString()}";
                break;
            case 6:
                MinigameID = $"Minigame_Trash_{defaultDifficulty.ToString()}";
                break;
            case 7:
                MinigameID = $"Minigame_Coin_{defaultDifficulty.ToString()}";
                break;
        }
        InitializeMinigame();
        return MinigameName;
    }

    public void InitializeMinigame()
    {
        MinigameStatus = Status.InProgress;
        var splitName = MinigameID.Split('_');
        MinigameName = $"{splitName[0]}_{splitName[1]}";
        MinigameDifficulty = splitName[2];
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
        LevelManager.NextLevelFlag = false;
        MinigameStatus = Status.Passed;
        _uiManager.TriggerMinigameEndMenu(true);
    }

    public void Fail()
    {
        LevelManager.NextLevelFlag = false;
        MinigameStatus = Status.Failed;
        _uiManager.TriggerMinigameEndMenu(false);
    }

    /// <summary>
    ///  Manipulates player attributes based on whether a minigame was passed or failed
    /// </summary>
    private void CheckPassOrFail()
    {
        switch (MinigameStatus)
        {
            case Status.Failed:
                LevelManager.NextLevelFlag = false;
                MinigameStatus = Status.None;
                PlayerController.Lives -= 1;
                if (PlayerController.Lives != 0)
                    _saveManager.Save();
                break;
            case Status.Passed:
                LevelManager.NextLevelFlag = false;
                MinigameStatus = Status.None;
                PlayerController.Points += MinigamePoints[MinigameID];
                _levelManager.scoreboard["minigameBonus"] += MinigamePoints[MinigameID];
                MinigameID = ""; // Clears the variable so the minigame cannot be replayed
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
