using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    private static bool _failFlag;
    private static bool _passFlag;
    
    public static readonly Dictionary<string, int> MinigamePoints = new Dictionary<string, int>()
    {
        {"Minigame_Grandma_Easy", 50},
        {"Minigame_Grandma_Medium", 75},
        {"Minigame_Grandma_Hard", 100},
        {"Minigame_Trash_Easy", 50},
        {"Minigame_Trash_Medium", 75},
        {"Minigame_Trash_Hard", 100},
        {"Minigame_Word_Easy", 50},
        {"Minigame_Word_Medium", 75},
        {"Minigame_Word_Hard", 100},
    };
    public static string Minigame = ""; // Full id of minigame e.g., Minigame_Grandma_Easy
    public static string MinigameName = ""; // First two parts of the minigame id e.g., Minigame_Grandma
    public static string MinigameDifficulty = ""; // Just the last part of the minigame id containing the difficulty
    public static int MinigameProgression; // Count of minigames passed on a level by level basis

    public static string ResolveEmptyMinigame() // For debug purposes
    {
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 5:
                Minigame = "Minigame_Grandma_Easy";
                break;
            case 6:
                Minigame = "Minigame_Trash_Easy";
                break;
            case 7:
                Minigame = "Minigame_Coin_Easy";
                break;
        }
        InitializeMinigame();
        return MinigameName;
    }

    public static void InitializeMinigame()
    {
        var splitName = Minigame.Split('_');
        MinigameName = $"{splitName[0]}_{splitName[1]}";
        MinigameDifficulty = splitName[2];
        SaveManager.Save();
    }

    public void Pass()
    {
        _passFlag = true;
        UIManager.TriggerMinigameEndMenu(true);
    }

    public void Fail()
    {
        _failFlag = true;
        UIManager.TriggerMinigameEndMenu(false);
    }

    /// <summary>
    ///  Manipulates player attributes based on whether a minigame was passed or failed
    /// </summary>
    private void CheckPassOrFail()
    {
        if (_failFlag)
        {
            _failFlag = false;
            PlayerController.Lives -= 1;
            if (PlayerController.Lives != 0)
                SaveManager.Save();
        } else if (_passFlag)
        {
            _passFlag = false;
            PlayerController.Points += MinigamePoints[Minigame];
            LevelManager.Scoreboard["minigameBonus"] += MinigamePoints[Minigame];
            Minigame = ""; // Clears the variable so the minigame cannot be replayed
            MinigameProgression++;
            SaveManager.Save();
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex >= 5) return;
        CheckPassOrFail();
    }
}
