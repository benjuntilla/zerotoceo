using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private SaveManager _saveManager;
    private static NPCController _managerNPCController;
    private PlayerController _playerController;
    private CharactersManager _charactersManager;

    public Dictionary<string, int> scoreboard;
    public int LevelIndex;
    public static bool NextLevelFlag;
    public Dictionary<int, int> nextLevelRequirements;
    
    public static LevelType CurrentLevelType;

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
        _charactersManager = GetComponent<CharactersManager>();
        _saveManager = GetComponent<SaveManager>();
        LevelIndex = SceneManager.GetActiveScene().buildIndex;
        nextLevelRequirements = new Dictionary<int, int>()
        {
            {1, levelOne},
            {2, levelTwo},
            {3, levelThree},
            {4, levelFour}
        };
        scoreboard = new Dictionary<string, int>()
        {
            {"dialogueBonus", 0},
            {"dialoguePenalty", 0},
            {"minigameBonus", 0}
        };
        
        // Set level type
        var level = SceneManager.GetActiveScene().buildIndex;
        if (level == 0)
        {
            CurrentLevelType = LevelType.Menu;
        }
        else if (level == 1 || level == 2 || level == 3 || level == 4)
        {
            CurrentLevelType = LevelType.Level;
        }
        else if (level == 5 || level == 6 || level == 7)
        {
            CurrentLevelType = LevelType.Minigame;
        }

        // Get PlayerController when needed
        if (CurrentLevelType == LevelType.Level)
        {
            _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        if (NextLevelFlag)
            InitializeNextLevel();
    }
    
    public void InitializeNextLevel()
    {
        _playerController.IncrementLives(1);
        _charactersManager.TriggerManagerDialogue();
        _saveManager.Save();
        NextLevelFlag = false;
    }

    public static void LoadLevelIndex(int index)
    {
        SceneManager.LoadScene(index);
    }
    
    public static void LoadLevelName(string name)
    {
        SceneManager.LoadScene(name);
    }
}
