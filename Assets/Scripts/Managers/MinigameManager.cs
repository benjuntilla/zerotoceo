using System;
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
    
    public static MinigameInfo minigameInfo { get; private set; } = new MinigameInfo();
    [HideInInspector] public int minigameProgression;
    [Header("Minigame Config")]
    public Difficulty defaultDifficulty = Difficulty.Easy;
    public int easyPointGain, mediumPointGain, hardPointGain;
    
    public class MinigameInfo
    {
        public readonly string id;
        public readonly string name;
        public readonly Difficulty difficulty;
        public Status status { get; private set; }
        public void SetStatus(Status set)
        {
            status = set; 
        }
        public MinigameInfo(string id = null, Status status = Status.None)
        {
            this.status = status;
            if (string.IsNullOrEmpty(id)) return;
            var split = id.Split('_');
            this.id = id;
            name = $"{split[0]}_{split[1]}";
            difficulty = (Difficulty) Enum.Parse(typeof(Difficulty), split[2]);
        }
    }
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

        CheckPassOrFail();
    }
    
    private void CheckPassOrFail()
    {
        if (minigameInfo.status == Status.Failed)
        {
            minigameInfo.SetStatus(Status.None);
            _player.lives--;
            if (_player.lives != 0)
                _saveManager.Save();
        }
        else if (minigameInfo.status == Status.Passed)
        {
            minigameInfo.SetStatus(Status.None);
            _player.points += CurrentPotentialPointGain();
            _levelManager.scoreboard["minigameBonus"] += CurrentPotentialPointGain();
            minigameInfo = new MinigameInfo();
            minigameProgression++;
            _saveManager.Save();
        }
    }

    public string ResolveEmptyMinigame()
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 5:
                minigameInfo = new MinigameInfo($"Minigame_Grandma_{defaultDifficulty.ToString()}", Status.InProgress);
                break;
            case 6:
                minigameInfo = new MinigameInfo($"Minigame_Trash{defaultDifficulty.ToString()}", Status.InProgress);
                break;
            case 7:
                minigameInfo = new MinigameInfo($"Minigame_Coin{defaultDifficulty.ToString()}", Status.InProgress);
                break;
        }
        PrepareMinigame();
        return minigameInfo.name;
    }

    public void PrepareMinigame()
    {
        minigameInfo.SetStatus(Status.InProgress);
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
        if (minigameInfo.status != Status.InProgress) return;
        minigameInfo.SetStatus(Status.Passed);
        _minigame.StopTimer();
        _returningFromMinigame = true;
        _modal.Trigger("minigamePass");
    }

    public void Fail()
    {
        if (minigameInfo.status != Status.InProgress) return;
        minigameInfo.SetStatus(Status.Failed);
        _minigame.StopTimer();
        _returningFromMinigame = true;
        _modal.Trigger("minigameFail");
    }

    public static void SetMinigame(string id, Status status)
    {
        if (!_returningFromMinigame) minigameInfo = new MinigameInfo(id, status);
    }

    public static void ResetMinigame()
    {
        minigameInfo = new MinigameInfo();
    }

    public int CurrentPotentialPointGain()
    {
        return PotentialPointGain(minigameInfo.id);
    }

    public int PotentialPointGain(string id)
    {
        var minigame = new MinigameInfo(id);
        switch (minigame.difficulty)
        {
            case Difficulty.Easy:
                return easyPointGain;
            case Difficulty.Medium:
                return mediumPointGain;
            case Difficulty.Hard:
                return hardPointGain;
            default:
                return 0;
        }
    }

    public void TryTriggerMinigame(Action cb)
    {
        if (minigameInfo.id != "") _modal.Trigger("minigame");
        else cb.Invoke();
    }
}
