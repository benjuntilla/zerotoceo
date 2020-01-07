using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class LevelManager : MonoBehaviour
{
    public static Dictionary<string, int> Scoreboard;
    public static int Level;
    public static bool NextLevelFlag;
    public static Dictionary<int, int> NextLevelRequirements;

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
        
        if (!NextLevelFlag) return;
        InitializeNextLevel();
    }
    
    public static void InitializeNextLevel()
    {
        GameObject.Find("Manager").GetComponent<NPCController>().dialogueTriggered = true;
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
