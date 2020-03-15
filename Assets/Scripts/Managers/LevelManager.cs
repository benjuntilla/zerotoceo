using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SaveManager), typeof(CharactersManager))]
public class LevelManager : MonoBehaviour
{
    private SaveManager _saveManager;
    private CharactersManager _charactersManager;
    private Player _player;
    private static bool _nextLevelFlag;

    public Dictionary<string, int> scoreboard = new Dictionary<string, int>()
    {
        {"dialogueBonus", 0},
        {"dialoguePenalty", 0},
        {"minigameBonus", 0}
    };
    public int levelIndex;
    public string levelName;
    public Dictionary<int, int> nextLevelRequirements;
    public LevelType currentLevelType;

    [Header("Level XP Requirements")]
    public int levelOne = 100;
    public int levelTwo = 200;
    public int levelThree = 300;
    public int levelFour = 400;

    public enum LevelType
    {
        Menu,
        Level,
        Minigame
    }
    
    void Awake()
    {
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        levelName = SceneManager.GetActiveScene().name;
        nextLevelRequirements = new Dictionary<int, int>()
        {
            {1, levelOne},
            {2, levelTwo},
            {3, levelThree},
            {4, levelFour}
        };
        
        // Set level type
        var level = SceneManager.GetActiveScene().buildIndex;
        if (level == 0)
        {
            currentLevelType = LevelType.Menu;
        }
        else if (level == 1 || level == 2 || level == 3 || level == 4)
        {
            currentLevelType = LevelType.Level;
        }
        else if (level == 5 || level == 6 || level == 7)
        {
            currentLevelType = LevelType.Minigame;
        }
    }

    void Start()
    {
        _charactersManager = GetComponent<CharactersManager>();
        _saveManager = GetComponent<SaveManager>();
        _player = FindObjectOfType<Player>();
        
        // Initialize next level when needed
        if (_nextLevelFlag)
            InitializeNextLevel();
    }

    public void InitializeNextLevel()
    {
        _charactersManager.TriggerManagerDialogue();
        _saveManager.Save();
        _nextLevelFlag = false;
    }

    public void LoadLevel(object param)
    {
        _nextLevelFlag = false;
        _saveManager.DisableLoadFlag();
        if (param is string)
            SceneManager.LoadScene((string) param);
        else if (param is int)
            SceneManager.LoadScene((int) param);
    }

    public void LoadNextLevel()
    {
        _nextLevelFlag = true;
        _saveManager.DisableLoadFlag();
        SceneManager.LoadScene(levelIndex++);
    }

    public void LoadSavedLevel()
    {
        _nextLevelFlag = false;
        _saveManager.EnableLoadFlag();
        SceneManager.LoadScene(_saveManager.GetSavedLevelIndex());
    }
}
