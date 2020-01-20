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
    public static Level LevelType;

    [Header("Level XP Requirements")]
    public int levelOne = 100;
    public int levelTwo = 200;
    public int levelThree = 300;
    public int levelFour = 400;

    public enum Level
    {
        Menu,
        Game,
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
            LevelType = Level.Menu;
        }
        else if (level == 1 || level == 2 || level == 3 || level == 4)
        {
            LevelType = Level.Game;
        }
        else if (level == 5 || level == 6 || level == 7)
        {
            LevelType = Level.Minigame;
        }
        
        if (LevelType == Level.Game && level != 4)
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
