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
    private PlayerController _playerController;
    
    public UnityEvent interactionMethods;
    public TextAsset dialogue;

    void Awake()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _levelManager = FindObjectOfType<LevelManager>();
        _charactersManager = FindObjectOfType<CharactersManager>();
        _uiManager = FindObjectOfType<UIManager>();
        _indicator = gameObject.transform.Find("Indicator").gameObject;
        _playerController = FindObjectOfType<PlayerController>();
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
        if (_playerController.points >= _levelManager.nextLevelRequirements[_levelManager.levelIndex])
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
