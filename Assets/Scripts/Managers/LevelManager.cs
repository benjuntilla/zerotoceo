using System;
using System.Collections.Generic;
using Ink.Runtime;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(SaveManager), typeof(CharactersManager))]
public class LevelManager : MonoBehaviour
{
    private SaveManager _saveManager;
    private CharactersManager _charactersManager;
    private Player _player;
    private Modal _modal;
    private MinigameManager _minigameManager;
    private MenuFull _menuFull;
    private int _potentialExperience;
    private static bool _nextLevelFlag;

    public Dictionary<string, int> scoreboard = new Dictionary<string, int>()
    {
        {"dialogueBonus", 0},
        {"dialoguePenalty", 0},
        {"minigameBonus", 0}
    };
    public int levelIndex { get; private set; }
    public string levelName { get; private set; }
    public Dictionary<int, int> nextLevelRequirements { get; private set; }
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
        _charactersManager = GetComponent<CharactersManager>();
        _saveManager = GetComponent<SaveManager>();
        levelIndex = SceneManager.GetActiveScene().buildIndex;
        levelName = SceneManager.GetActiveScene().name;
        nextLevelRequirements = new Dictionary<int, int>()
        {
            {1, levelOne},
            {2, levelTwo},
            {3, levelThree},
            {4, levelFour}
        };
    }

    void Start()
    {
        _player = FindObjectOfType<Player>();
        _modal = FindObjectOfType<Modal>();
        _menuFull = FindObjectOfType<MenuFull>();
        _minigameManager = FindObjectOfType<MinigameManager>();
        
        CalculatePotentialPoints();
        if (_nextLevelFlag)
            InitializeNextLevel();
    }

    private void CalculatePotentialPoints()
    {
        var interactables = FindObjectsOfType<Interactable>();
        foreach (var interactable in interactables)
        {
            var dialogue = new Story(interactable.dialogue.text);
            if (dialogue.globalTags.Count > 1)
                _potentialExperience += Int32.Parse(dialogue.globalTags[1]);
            if (dialogue.globalTags.Count > 2)
                _potentialExperience += _minigameManager.PotentialPointGain(dialogue.globalTags[2]);
        }
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
        levelIndex++;
        SceneManager.LoadScene(levelIndex);
    }

    public void LoadSavedLevel()
    {
        _nextLevelFlag = false;
        _saveManager.EnableLoadFlag();
        SceneManager.LoadScene(_saveManager.GetSavedLevelIndex());
    }

    public void TryLevelUp(Action cb)
    {
        if (_player.points >= nextLevelRequirements[levelIndex]) _menuFull.Trigger("levelEnd");
        else cb.Invoke();
    }

    private void Update()
    {
        if (_potentialExperience - scoreboard["dialoguePenalty"] < nextLevelRequirements[levelIndex]) _modal.Trigger("softLock");
    }
}
