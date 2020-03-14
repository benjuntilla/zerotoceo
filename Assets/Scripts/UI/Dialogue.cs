using TMPro;
using UnityEngine;

namespace UI
{
    public class Dialogue : UIObject
    {
        public GameObject primaryButtonObject, secondaryButtonObject, tertiaryButtonObject;
        public TextMeshProUGUI titleText, bodyText;

        private TextMeshProUGUI _primaryButtonText, _secondaryButtonText, _tertiaryButtonText;
        private DialogueManager _dialogueManager;
        private Coroutine _typeCoroutine;
        private RectTransform _rectTransform;
        private string _currentText;

        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _primaryButtonText = primaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();
            _secondaryButtonText = secondaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();
            _tertiaryButtonText = tertiaryButtonObject.GetComponentInChildren<TextMeshProUGUI>();

            _dialogueManager = FindObjectOfType<DialogueManager>();
        }

        private void TypeDialogue(string text)
        {
            if (_typeCoroutine != null)
                StopCoroutine(_typeCoroutine);
            _typeCoroutine = StartCoroutine(Helper.TypeText(bodyText, text));
        }

        private void UpdatePosition()
        {
            _rectTransform.anchoredPosition = new Vector2(0, _rectTransform.rect.height/2);
        }

        private void UpdateUI()
        {
            if (!_dialogueManager || _dialogueManager.currentDialogueName == "")
            {
                Disable();
            }
            else if (!_dialogueManager.dialogue.canContinue && _dialogueManager.dialogue.currentChoices.Count > 0) // If a dialogue choice(s) is possible
            {
                Enable();
                titleText.SetText(_dialogueManager.dialogue.globalTags[0]);
                // Set UI layout & fill text
                switch (_dialogueManager.dialogue.currentChoices.Count)
                {
                    case 1:
                        primaryButtonObject.SetActive(true);
                        secondaryButtonObject.SetActive(false);
                        tertiaryButtonObject.SetActive(false);
                        _primaryButtonText.SetText(_dialogueManager.dialogue.currentChoices[0].text);
                        break;
                    case 2:
                        primaryButtonObject.SetActive(true);
                        secondaryButtonObject.SetActive(true);
                        tertiaryButtonObject.SetActive(false);
                        _primaryButtonText.SetText(_dialogueManager.dialogue.currentChoices[0].text);
                        _secondaryButtonText.SetText(_dialogueManager.dialogue.currentChoices[1].text);
                        break;
                    case 3:
                        primaryButtonObject.SetActive(true);
                        secondaryButtonObject.SetActive(true);
                        tertiaryButtonObject.SetActive(true);
                        _primaryButtonText.SetText(_dialogueManager.dialogue.currentChoices[0].text);
                        _secondaryButtonText.SetText(_dialogueManager.dialogue.currentChoices[1].text);
                        _tertiaryButtonText.SetText(_dialogueManager.dialogue.currentChoices[2].text);
                        break;
                }
            } 
            else
            {
                Enable();
                titleText.SetText(_dialogueManager.dialogue.globalTags[0]);
                // Set UI layout & fill text
                primaryButtonObject.SetActive(true);
                secondaryButtonObject.SetActive(false);
                tertiaryButtonObject.SetActive(false);
                _primaryButtonText.SetText("Continue");
            }
        }

        private void UpdateText()
        {
            if (!_dialogueManager || _dialogueManager.dialogue == null || _dialogueManager.dialogue.currentText == _currentText) return;
            _currentText = _dialogueManager.dialogue.currentText;
            TypeDialogue(_currentText);
        }

        public void ClickPrimaryButton()
        {
            _dialogueManager.ClickPrimaryButton();
        }

        public void ClickSecondaryButton()
        {
            _dialogueManager.ClickSecondaryButton();
        }

        public void ClickTertiaryButton()
        {
            _dialogueManager.ClickTertiaryButton();
        }

        protected override void Update()
        {
            base.Update();
            UpdatePosition();
            UpdateUI();
            UpdateText();
        }
    }
}
