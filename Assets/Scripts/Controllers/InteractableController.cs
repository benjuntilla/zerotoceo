using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableController : MonoBehaviour
{
    private UIManager _uiManager;
    private LevelManager _levelManager;
    private DialogueManager _dialogueManager;
    private CharactersManager _charactersManager;
    private GameObject _indicator;
    
    public UnityEvent interactionMethods;
    public TextAsset dialogue;

    void Awake()
    {
        var gameManagers = GameObject.FindWithTag("GameManagers");
        _dialogueManager = gameManagers.GetComponent<DialogueManager>();
        _levelManager = gameManagers.GetComponent<LevelManager>();
        _charactersManager = gameManagers.GetComponent<CharactersManager>();
        _uiManager = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        _indicator = gameObject.transform.Find("Indicator").gameObject;
    }

    public void Interact()
    {
        interactionMethods.Invoke();
    }

    public void TestIfMinigame()
    {
        if (MinigameManager.minigameId != "")
            _uiManager.TriggerModal("minigame");
        else
            TriggerDialogue();
    }
    
    public void TestIfCanLevelUp()
    {
        if (PlayerController.Points >= _levelManager.nextLevelRequirements[_levelManager.levelIndex])
            _uiManager.TriggerLevelEndMenu();
        else
            TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        _dialogueManager.TriggerDialogue(dialogue);
    }

    void Update()
    {
        _indicator.SetActive(_charactersManager.indicatorTarget == gameObject);
    }
}
