using Ink.Runtime;
using System.Collections.Generic;
using UI;
using UnityEngine;

[RequireComponent(typeof(MinigameManager), typeof(LevelManager))]
public class DialogueManager : MonoBehaviour
{
    public string currentDialogueName = "";
    public Dictionary<string, string> sessionDialogueData = new Dictionary<string, string>(); // This field stores individual dialogue progression data
    public InkList gameFlags = new InkList(); // This field stores data used by all dialogues
    public Story dialogue { get; private set; }

    private LevelManager _levelManager;
    private MinigameManager _minigameManager;
    private PlayerController _playerController;
    private Popup _popup;

    void Start()
    {
        _levelManager = GetComponent<LevelManager>();
        _minigameManager = GetComponent<MinigameManager>();
        _playerController = FindObjectOfType<PlayerController>();
        _popup = FindObjectOfType<Popup>();
    }
    public void TriggerDialogue(TextAsset inkAsset)
    {
        // Set-up
        dialogue = new Story(inkAsset.text);
        currentDialogueName = inkAsset.name;
        if (sessionDialogueData.ContainsKey(currentDialogueName)) 
            dialogue.state.LoadJson(sessionDialogueData[currentDialogueName]);
        dialogue.ChoosePathString("start");

        // Set external functions and observe needed variables
        dialogue.BindExternalFunction("GetGameLevel", () => _levelManager.levelIndex);
        dialogue.BindExternalFunction("GetPlayerXP", () => _playerController.points);
        dialogue.BindExternalFunction("GetRequiredPoints", () => _levelManager.nextLevelRequirements[_levelManager.levelIndex]);
        dialogue.BindExternalFunction("GetMinigameProgression", () => _minigameManager.minigameProgression);
        if (dialogue.variablesState["xp"] != null)
        {
            dialogue.ObserveVariable("xp", (varName, newValue) =>
            {
                if ((int) newValue > _playerController.points)
                    _levelManager.scoreboard["dialogueBonus"] += (int) newValue - _playerController.points;
                else
                    _levelManager.scoreboard["dialoguePenalty"] += _playerController.points - (int) newValue;
                _playerController.points = (int) newValue;
            });
        }
        if (dialogue.variablesState["pendingMinigame"] != null)
        {
            dialogue.ObserveVariable("pendingMinigame", (varName, newValue) =>
            {
                MinigameManager.minigameStatus = MinigameManager.Status.Pending;
                MinigameManager.minigameId = (string) newValue;
                _popup.Queue("minigame");
            });
        }
        if (dialogue.variablesState["gameFlags"] != null)
        {
            if (gameFlags.Count != 0)
                dialogue.variablesState["gameFlags"] = gameFlags;
            dialogue.ObserveVariable("gameFlags", (varName, newValue) =>
            {
                gameFlags = (InkList) newValue;
            });
        }

        ContinueDialogue();
    }

    private void ContinueDialogue()
    {
        if (dialogue.canContinue)
            dialogue.Continue(); // The Continue() method both progresses and returns the current dialogue text
        else if (dialogue.currentChoices.Count == 0)
            EndDialogue();
    }

    private void ChooseChoice(int choice)
    {
        dialogue.ChooseChoiceIndex(choice);
        ContinueDialogue();
    }
    
    public void ClickPrimaryButton()
    {
        if (!dialogue.canContinue && dialogue.currentChoices.Count > 0)
            ChooseChoice(0);
        else
            ContinueDialogue();
    }
    
    public void ClickSecondaryButton()
    {
        ChooseChoice(1);
    }
    
    public void ClickTertiaryButton()
    {
        ChooseChoice(2);
    }

    private void EndDialogue()
    {
        sessionDialogueData[currentDialogueName] = dialogue.state.ToJson();
        dialogue = null;
        currentDialogueName = "";
    }
}
