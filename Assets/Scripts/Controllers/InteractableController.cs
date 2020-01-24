using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableController : MonoBehaviour
{
    private UIManager _uiManager;
    
    public UnityEvent interactionMethods;
    public TextAsset dialogue;

    void Awake()
    {
        _uiManager = GameObject.FindWithTag("UI").GetComponent<UIManager>();
    }

    public void Interact()
    {
        interactionMethods.Invoke();
    }

    public void TestIfMinigame()
    {
        if (MinigameManager.MinigameID != "")
            _uiManager.TriggerModal("minigame");
        else
            TriggerDialogue();
    }
    
    public void TestIfCanLevelUp()
    {
        if (PlayerController.Points >= LevelManager.NextLevelRequirements[LevelManager.LevelIndex])
        {
            _uiManager.TriggerLevelEndMenu();
        }
        else
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        DialogueManager.TriggerDialogue(dialogue);
    }
}
