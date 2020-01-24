using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static NPCController _managerNPCController;

    public static Dictionary<string, int> Scoreboard;
    public static int LevelIndex;
    public static bool NextLevelFlag;
    public static Dictionary<int, int> NextLevelRequirements;
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
        ClearScoreboard();
        LevelIndex = SceneManager.GetActiveScene().buildIndex;
        NextLevelRequirements = new Dictionary<int, int>()
        {
            {1, levelOne},
            {2, levelTwo},
            {3, levelThree},
            {4, levelFour}
        };

        var level = SceneManager.GetActiveScene().buildIndex;
        // Set level type
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
        
        if (CurrentLevelType == LevelType.Level && level != 4)
        {
            _managerNPCController = GameObject.Find("Manager").GetComponent<NPCController>();
        }

        if (NextLevelFlag)
        {
            InitializeNextLevel();
        }
    }
    
    public static void InitializeNextLevel()
    {
        if (PlayerController.Lives < 3)
            PlayerController.Lives++;
        _managerNPCController.dialogueTriggered = true;
        MinigameManager.MinigameProgression = 0;
        SaveManager.Save();
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

    public static void ClearScoreboard()
    {
        Scoreboard = new Dictionary<string, int>()
        {
            {"dialogueBonus", 0},
            {"dialoguePenalty", 0},
            {"minigameBonus", 0}
        };
    }
}
