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
    private HUD _hud;
    private static bool _returningFromMinigame;
    
    public static string minigameId { get; private set; } = ""; // Full id of minigame e.g., Minigame_Grandma_Easy
    public static string minigameName { get; private set; } = ""; // First two parts of the minigame id e.g., Minigame_Grandma
    public static string minigameDifficulty { get; private set; }= ""; // Just the last part of the minigame id containing the difficulty
    public static Status minigameStatus { get; private set; } = Status.None;
    [HideInInspector] public int minigameProgression; // Count of minigames passed on a level by level basis
    [Header("Minigame Config")]
    public Difficulty defaultDifficulty = Difficulty.Easy; // Used for debugging
    public int easyPointGain, mediumPointGain, hardPointGain;
    
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
        _hud = FindObjectOfType<HUD>();
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
            _hud.StartCoroutine(_hud.TriggerCountdown(_minigame.OnMinigameStart));
        else
            _minigame.OnMinigameStart();
    }

    public void Pass()
    {
        if (minigameStatus != Status.InProgress) return;
        minigameStatus = Status.Passed;
        _minigame.StopTimer();
        _returningFromMinigame = true;
        _modal.Trigger("minigamePass");
    }

    public void Fail()
    {
        if (minigameStatus != Status.InProgress) return;
        minigameStatus = Status.Failed;
        _minigame.StopTimer();
        _returningFromMinigame = true;
        _modal.Trigger("minigameFail");
    }

    public void SetMinigameStatus(Status status)
    {
        if (_returningFromMinigame) return;
        minigameStatus = status;
    }

    public void SetMinigameID(string id)
    {
        minigameId = id;
    }

    public int PotentialPointGain()
    {
        switch (minigameDifficulty)
        {
            case "Easy":
                return easyPointGain;
            case "Medium":
                return mediumPointGain;
            case "Hard":
                return hardPointGain;
            default:
                return 0;
        }
    }

    private void CheckPassOrFail()
    {
        if (minigameStatus == Status.Failed)
        {
            minigameStatus = Status.None;
            _player.lives--;
            if (_player.lives != 0)
                _saveManager.Save();
        }
        else if (minigameStatus == Status.Passed)
        {
            minigameStatus = Status.None;
            _player.points += PotentialPointGain();
            _levelManager.scoreboard["minigameBonus"] += PotentialPointGain();
            minigameId = ""; // Clears the variable so the minigame cannot be replayed
            minigameProgression++;
            _saveManager.Save();
        }
    }

    private void Update()
    {
        if (_levelManager.currentLevelType != LevelManager.LevelType.Level) return;
        CheckPassOrFail();
    }
}
