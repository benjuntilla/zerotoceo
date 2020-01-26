using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UIManager), typeof(MinigameManager), typeof(LevelManager))]
public class DialogueManager : MonoBehaviour
{
    public string currentDialogue = "";
    public Dictionary<string, string> sessionDialogueData = new Dictionary<string, string>(); // This field stores individual dialogue progression data
    public InkList gameFlags = new InkList(); // This field stores data used by all dialogues

    private Story _dialogue, _genericDialogue;
    public GameObject ui, dialogueUI, primaryButtonObject, secondaryButtonObject, tertiaryButtonObject;
    public TextMeshProUGUI primaryButtonText, secondaryButtonText, tertiaryButtonText, titleText, dialogueText;
    public Button primaryButton, secondaryButton, tertiaryButton;
    private UIManager _uiManager;
    private LevelManager _levelManager;
    private MinigameManager _minigameManager;
    private PlayerController _playerController;

    void Awake()
    {
        ui = GameObject.FindWithTag("UI");
        dialogueUI = ui.transform.Find("Dialogue").gameObject;
        titleText = dialogueUI.transform.Find("Title Text").gameObject.GetComponent<TextMeshProUGUI>();
        dialogueText = dialogueUI.transform.Find("Dialogue Text").gameObject.GetComponent<TextMeshProUGUI>();
        primaryButtonObject = dialogueUI.transform.Find("Primary Button").gameObject;
        secondaryButtonObject = dialogueUI.transform.Find("Secondary Button").gameObject;
        tertiaryButtonObject = dialogueUI.transform.Find("Tertiary Button").gameObject;
        primaryButtonText = primaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();
        secondaryButtonText = secondaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();
        tertiaryButtonText = tertiaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();
        primaryButton = primaryButtonObject.GetComponent<Button>();
        secondaryButton = secondaryButtonObject.GetComponent<Button>();
        tertiaryButton = tertiaryButtonObject.GetComponent<Button>();

        _uiManager = ui.GetComponent<UIManager>();
        _levelManager = GetComponent<LevelManager>();
        _minigameManager = GetComponent<MinigameManager>();
        _playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        
        dialogueUI.SetActive(false);
    }
    public void TriggerDialogue(TextAsset inkAsset)
    {
        // Set-up
        _dialogue = new Story(inkAsset.text);
        currentDialogue = inkAsset.name;
        if (sessionDialogueData.ContainsKey(currentDialogue)) 
            _dialogue.state.LoadJson(sessionDialogueData[currentDialogue]);
        _dialogue.ChoosePathString("start");

        // Set external functions and observe xp variable if possible
        _dialogue.BindExternalFunction("GetGameLevel", () => _levelManager.levelIndex);
        _dialogue.BindExternalFunction("GetPlayerXP", () => PlayerController.points);
        _dialogue.BindExternalFunction("GetRequiredPoints", () => _levelManager.nextLevelRequirements[_levelManager.levelIndex]);
        _dialogue.BindExternalFunction("GetMinigameProgression", () => _minigameManager.minigameProgression);
        if (_dialogue.variablesState["xp"] != null)
        {
            _dialogue.ObserveVariable("xp", (varName, newValue) =>
            {
                if ((int) newValue > PlayerController.points)
                    _levelManager.scoreboard["dialogueBonus"] += (int) newValue - PlayerController.points;
                else
                    _levelManager.scoreboard["dialoguePenalty"] += PlayerController.points - (int) newValue;
                _playerController.SetPoints((int) newValue);
            });
        }
        if (_dialogue.variablesState["pendingMinigame"] != null)
        {
            _dialogue.ObserveVariable("pendingMinigame", (varName, newValue) =>
            {
                MinigameManager.minigameStatus = MinigameManager.Status.Pending;
                MinigameManager.minigameId = (string) newValue;
                _uiManager.QueuePopup("minigame");
            });
        }
        if (_dialogue.variablesState["gameFlags"] != null)
        {
            if (gameFlags.Count != 0)
                _dialogue.variablesState["gameFlags"] = gameFlags;
            _dialogue.ObserveVariable("gameFlags", (varName, newValue) =>
            {
                gameFlags = (InkList) newValue;
            });
        }

        // Enable dialogue UI and fill title
        dialogueUI.SetActive(true);
        titleText.SetText(_dialogue.globalTags[0]);

        // Begin dialogue
        ContinueDialogue();
    }

    private void ContinueDialogue()
    {
        if (_dialogue.canContinue)
        {
            StopAllCoroutines();
            StartCoroutine(TypeDialogue(_dialogue.Continue())); // The Continue() method both progresses and returns the current dialogue text
        } else if (_dialogue.currentChoices.Count == 0)
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeDialogue(string dialogue)
    {
        var text = "";
        dialogueText.SetText("");
        foreach (var letter in dialogue.ToCharArray())
        {
            text += letter;
            dialogueText.SetText(text);
            yield return null; // Wait a frame
        }
    }

    private void ChooseChoice(int choice)
    {
        _dialogue.ChooseChoiceIndex(choice);
        ContinueDialogue();
    }

    private void EndDialogue()
    {
        sessionDialogueData[currentDialogue] = _dialogue.state.ToJson();
        _dialogue = null;
        currentDialogue = "";
        dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (!_dialogue) return;
        if (!_dialogue.canContinue && _dialogue.currentChoices.Count > 0) // If a dialogue choice(s) is possible
        {
            // Set UI layout & fill text
            switch (_dialogue.currentChoices.Count)
            {
                case 1:
                    primaryButtonObject.SetActive(true);
                    secondaryButtonObject.SetActive(false);
                    tertiaryButtonObject.SetActive(false);
                    primaryButtonText.SetText(_dialogue.currentChoices[0].text);
                    break;
                case 2:
                    primaryButtonObject.SetActive(true);
                    secondaryButtonObject.SetActive(true);
                    tertiaryButtonObject.SetActive(false);
                    primaryButtonText.SetText(_dialogue.currentChoices[0].text);
                    secondaryButtonText.SetText(_dialogue.currentChoices[1].text);
                    break;
                case 3:
                    primaryButtonObject.SetActive(true);
                    secondaryButtonObject.SetActive(true);
                    tertiaryButtonObject.SetActive(true);
                    primaryButtonText.SetText(_dialogue.currentChoices[0].text);
                    secondaryButtonText.SetText(_dialogue.currentChoices[1].text);
                    tertiaryButtonText.SetText(_dialogue.currentChoices[2].text);
                    break;
            }
            // Set UI event listeners
            primaryButton.onClick.RemoveAllListeners();
            primaryButton.onClick.AddListener(delegate { ChooseChoice(0); });
            secondaryButton.onClick.RemoveAllListeners();
            secondaryButton.onClick.AddListener(delegate { ChooseChoice(1); });
            tertiaryButton.onClick.RemoveAllListeners();
            tertiaryButton.onClick.AddListener(delegate { ChooseChoice(2); });
        } else
        {
            // Set UI layout & fill text
            primaryButtonObject.SetActive(true);
            secondaryButtonObject.SetActive(false);
            tertiaryButtonObject.SetActive(false);
            primaryButtonText.SetText("Continue");
            // Set UI event listeners
            primaryButton.onClick.RemoveAllListeners();
            primaryButton.onClick.AddListener(ContinueDialogue);
        }
    }
}
