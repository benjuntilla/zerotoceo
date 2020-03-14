﻿using UI;
using UnityEngine;
using UnityEngine.Events;

public class InteractableController : MonoBehaviour
{
    private LevelManager _levelManager;
    private DialogueManager _dialogueManager;
    private CharactersManager _charactersManager;
    private GameObject _indicator;
    private PlayerController _playerController;
    private Modal _modal;
    private MenuFull _menuFull;
    
    public UnityEvent interactionMethods;
    public TextAsset dialogue;

    void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _levelManager = FindObjectOfType<LevelManager>();
        _charactersManager = FindObjectOfType<CharactersManager>();
        _indicator = gameObject.transform.Find("Indicator").gameObject;
        _playerController = FindObjectOfType<PlayerController>();
        _modal = FindObjectOfType<Modal>();
        _menuFull = FindObjectOfType<MenuFull>();
    }

    public void Interact()
    {
        interactionMethods.Invoke();
    }

    public void TestIfMinigame()
    {
        if (MinigameManager.minigameId != "")
            _modal.Trigger("minigame");
        else
            TriggerDialogue();
    }
    
    public void TestIfCanLevelUp()
    {
        if (_playerController.points >= _levelManager.nextLevelRequirements[_levelManager.levelIndex])
            _menuFull.Trigger("levelEnd");
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
