using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static string CurrentDialogue = "";
    public static Dictionary<string, string> SessionDialogueData = new Dictionary<string, string>();

    private static Story _dialogue;
    private static GameObject _ui, _dialogueUI, _primaryButtonObject, _secondaryButtonObject, _tertiaryButtonObject;
    private static TextMeshProUGUI _primaryButtonText, _secondaryButtonText, _tertiaryButtonText, _titleText, _dialogueText;
    private static Button _primaryButton, _secondaryButton, _tertiaryButton;
    private static DialogueManager _instance; // This allows non-static methods (e.g. coroutines) to be called in static methods via an instance of this class

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
        
        CurrentDialogue = "";
        _dialogueUI.SetActive(false);
        _instance = this;
    }
    public static void TriggerDialogue(TextAsset inkAsset)
    {
        // Set-up
        _dialogue = new Story(inkAsset.text);
        CurrentDialogue = inkAsset.name;
        if (SessionDialogueData.ContainsKey(CurrentDialogue)) 
            _dialogue.state.LoadJson(SessionDialogueData[CurrentDialogue]);
        _dialogue.ChoosePathString("start");

        // Set external functions and observe xp variable if possible
        _dialogue.BindExternalFunction("GetGameLevel", () => LevelManager.Level);
        _dialogue.BindExternalFunction("GetPlayerXP", () => PlayerController.Points);
        _dialogue.BindExternalFunction("GetRequiredPoints", () => LevelManager.NextLevelRequirements[LevelManager.Level]);
        if (_dialogue.variablesState["xp"] != null)
        {
            _dialogue.ObserveVariable("xp", (varName, newValue) =>
            {
                if ((int) newValue > PlayerController.Points)
                    LevelManager.Scoreboard["dialogueBonus"] += (int) newValue - PlayerController.Points;
                else
                    LevelManager.Scoreboard["dialoguePenalty"] += PlayerController.Points - (int) newValue;
                PlayerController.Points = (int) newValue;
            });
        }
        if (_dialogue.variablesState["pendingMinigame"] != null)
        {
            _dialogue.ObserveVariable("pendingMinigame", (varName, newValue) =>
            {
                MinigameManager.Minigame = (string) newValue;
                UIManager.TriggerPopup("minigame");
                // _dialogue.variablesState["pendingMinigame"] = ""; Causes a stack overflow
            });
        }

        // Enable dialogue UI and fill title
        _dialogueUI.SetActive(true);
        _titleText.SetText(_dialogue.globalTags[0]);

        // Begin dialogue
        ContinueDialogue();
    }

    private static void ContinueDialogue()
    {
        if (_dialogue.canContinue)
        {
            _instance.StopAllCoroutines();
            _instance.StartCoroutine(TypeDialogue(_dialogue.Continue())); // The Continue() method both progresses and returns the current dialogue text
        } else if (_dialogue.currentChoices.Count == 0)
        {
            EndDialogue();
        }
    }

    private static IEnumerator TypeDialogue(string dialogue)
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

    private static void ChooseChoice(int choice)
    {
        _dialogue.ChooseChoiceIndex(choice);
        ContinueDialogue();
    }

    private static void EndDialogue()
    {
        SessionDialogueData[CurrentDialogue] = _dialogue.state.ToJson();
        _dialogue = null;
        CurrentDialogue = "";
        _dialogueUI.SetActive(false);
    }

    void Update()
    {
        if (!_dialogue) return;
        if (!_dialogue.canContinue && _dialogue.currentChoices.Count > 0)
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
