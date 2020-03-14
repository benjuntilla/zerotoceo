using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SaveManager))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(Minigame))]
public class MinigameManager : MonoBehaviour
{
    private SaveManager _saveManager;
    private Minigame _minigame;
    private LevelManager _levelManager;
    private Player _player;
    private Modal _modal;

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

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _levelManager = GetComponent<LevelManager>();
        _saveManager = GetComponent<SaveManager>();
        _minigame = GetComponent<Minigame>();
        _modal = FindObjectOfType<Modal>();
    }

    public string ResolveEmptyMinigame()
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
        PrepareMinigame();
        return minigameName;
    }

    public void PrepareMinigame()
    {
        minigameStatus = Status.InProgress;
        var splitName = minigameId.Split('_');
        minigameName = $"{splitName[0]}_{splitName[1]}";
        minigameDifficulty = splitName[2];
        if (_saveManager != null)
            _saveManager.Save();
    }

    public void InitializeMinigame()
    {
        if (_minigame.timerStartTime != 0)
            _minigame.Invoke(nameof(Minigame.StartMinigame), 2);
        else
            _minigame.StartMinigame();
    }

    public void Pass()
    {
        if (minigameStatus != Status.InProgress) return;
        minigameStatus = Status.Passed;
        _modal.Trigger("minigamePass");
    }

    public void Fail()
    {
        if (minigameStatus != Status.InProgress) return;
        minigameStatus = Status.Failed;
        _modal.Trigger("minigameFail");
    }
    
    private void CheckPassOrFail()
    {
        switch (minigameStatus)
        {
            case Status.Failed:
                minigameStatus = Status.None;
                _player.lives--;
                if (_player.lives != 0)
                    _saveManager.Save();
                break;
            case Status.Passed:
                minigameStatus = Status.None;
                _player.lives += minigamePoints[minigameId];
                _levelManager.scoreboard["minigameBonus"] += minigamePoints[minigameId];
                minigameId = ""; // Clears the variable so the minigame cannot be replayed
                minigameProgression++;
                _saveManager.Save();
                break;
        }
    }

    private void Update()
    {
        if (_levelManager.currentLevelType != LevelManager.LevelType.Level) return;
        CheckPassOrFail();
    }
}
