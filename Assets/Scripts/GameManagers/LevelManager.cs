using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static Dictionary<string, int> Scoreboard;
    public static int Level;
    public static bool NextLevelFlag, IsMainGameLevel;
    public static Dictionary<int, int> NextLevelRequirements;
    private static NPCController _managerNPCController;

    [Header("Level XP Requirements")]
    public int levelOne = 100;
    public int levelTwo = 200;
    public int levelThree = 300;
    public int levelFour = 400;
    
    void Awake()
    {
        ClearScoreboard();
        Level = SceneManager.GetActiveScene().buildIndex;
        NextLevelRequirements = new Dictionary<int, int>()
        {
            {1, levelOne},
            {2, levelTwo},
            {3, levelThree},
            {4, levelFour}
        };

        var level = SceneManager.GetActiveScene().buildIndex;
        if (level == 1 || level == 2 || level == 3 || level == 4)
        {
            _managerNPCController = GameObject.Find("Manager").GetComponent<NPCController>();
            IsMainGameLevel = true;
        }
        else
        {
            IsMainGameLevel = false;
        }
        
        if (!NextLevelFlag) return;
        InitializeNextLevel();
    }
    
    public static void InitializeNextLevel()
    {
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
