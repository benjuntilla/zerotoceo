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
    private GameObject _ui, _dialogueUI, _primaryButtonObject, _secondaryButtonObject, _tertiaryButtonObject;
    private TextMeshProUGUI _primaryButtonText, _secondaryButtonText, _tertiaryButtonText, _titleText, _dialogueText;
    private Button _primaryButton, _secondaryButton, _tertiaryButton;
    private UIManager _uiManager;
    private LevelManager _levelManager;
    private MinigameManager _minigameManager;
    private PlayerController _playerController;

    void Awake()
    {
        _ui = GameObject.FindWithTag("UI");
        _dialogueUI = _ui.transform.Find("Dialogue").gameObject;
        _titleText = _dialogueUI.transform.Find("Title Text").gameObject.GetComponent<TextMeshProUGUI>();
        _dialogueText = _dialogueUI.transform.Find("Dialogue Text").gameObject.GetComponent<TextMeshProUGUI>();
        _primaryButtonObject = _dialogueUI.transform.Find("Primary Button").gameObject;
        _secondaryButtonObject = _dialogueUI.transform.Find("Secondary Button").gameObject;
        _tertiaryButtonObject = _dialogueUI.transform.Find("Tertiary Button").gameObject;
        _primaryButtonText = _primaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();
        _secondaryButtonText = _secondaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();
        _tertiaryButtonText = _tertiaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();
        _primaryButton = _primaryButtonObject.GetComponent<Button>();
        _secondaryButton = _secondaryButtonObject.GetComponent<Button>();
        _tertiaryButton = _tertiaryButtonObject.GetComponent<Button>();

        _uiManager = _ui.GetComponent<UIManager>();
        _levelManager = GetComponent<LevelManager>();
        _minigameManager = GetComponent<MinigameManager>();
        _playerController = FindObjectOfType<PlayerController>();
        
        _dialogueUI.SetActive(false);
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
        _dialogue.BindExternalFunction("GetPlayerXP", () => _playerController.points);
        _dialogue.BindExternalFunction("GetRequiredPoints", () => _levelManager.nextLevelRequirements[_levelManager.levelIndex]);
        _dialogue.BindExternalFunction("GetMinigameProgression", () => _minigameManager.minigameProgression);
        if (_dialogue.variablesState["xp"] != null)
        {
            _dialogue.ObserveVariable("xp", (varName, newValue) =>
            {
                if ((int) newValue > _playerController.points)
                    _levelManager.scoreboard["dialogueBonus"] += (int) newValue - _playerController.points;
                else
                    _levelManager.scoreboard["dialoguePenalty"] += _playerController.points - (int) newValue;
                _playerController.points = (int) newValue;
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
        _dialogueUI.SetActive(true);
        _titleText.SetText(_dialogue.globalTags[0]);

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
        _dialogueText.SetText("");
        foreach (var letter in dialogue.ToCharArray())
        {
            text += letter;
            _dialogueText.SetText(text);
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
        _dialogueUI.SetActive(false);
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
                    _primaryButtonObject.SetActive(true);
                    _secondaryButtonObject.SetActive(false);
                    _tertiaryButtonObject.SetActive(false);
                    _primaryButtonText.SetText(_dialogue.currentChoices[0].text);
                    break;
                case 2:
                    _primaryButtonObject.SetActive(true);
                    _secondaryButtonObject.SetActive(true);
                    _tertiaryButtonObject.SetActive(false);
                    _primaryButtonText.SetText(_dialogue.currentChoices[0].text);
                    _secondaryButtonText.SetText(_dialogue.currentChoices[1].text);
                    break;
                case 3:
                    _primaryButtonObject.SetActive(true);
                    _secondaryButtonObject.SetActive(true);
                    _tertiaryButtonObject.SetActive(true);
                    _primaryButtonText.SetText(_dialogue.currentChoices[0].text);
                    _secondaryButtonText.SetText(_dialogue.currentChoices[1].text);
                    _tertiaryButtonText.SetText(_dialogue.currentChoices[2].text);
                    break;
            }
            // Set UI event listeners
            _primaryButton.onClick.RemoveAllListeners();
            _primaryButton.onClick.AddListener(delegate { ChooseChoice(0); });
            _secondaryButton.onClick.RemoveAllListeners();
            _secondaryButton.onClick.AddListener(delegate { ChooseChoice(1); });
            _tertiaryButton.onClick.RemoveAllListeners();
            _tertiaryButton.onClick.AddListener(delegate { ChooseChoice(2); });
        } else
        {
            // Set UI layout & fill text
            _primaryButtonObject.SetActive(true);
            _secondaryButtonObject.SetActive(false);
            _tertiaryButtonObject.SetActive(false);
            _primaryButtonText.SetText("Continue");
            // Set UI event listeners
            _primaryButton.onClick.RemoveAllListeners();
            _primaryButton.onClick.AddListener(ContinueDialogue);
        }
    }
}
