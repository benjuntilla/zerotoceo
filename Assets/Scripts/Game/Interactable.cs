using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    private LevelManager _levelManager;
    private DialogueManager _dialogueManager;
    private CharactersManager _charactersManager;
    private GameObject _indicator;
    private MinigameManager _minigameManager;
    
    public UnityEvent interactionMethods;
    public TextAsset dialogue;

    private void Awake()
    {
        _indicator = gameObject.transform.Find("Indicator").gameObject;
    }

    private void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _levelManager = FindObjectOfType<LevelManager>();
        _charactersManager = FindObjectOfType<CharactersManager>();
        _minigameManager = FindObjectOfType<MinigameManager>();
    }

    public void Interact()
    {
        interactionMethods.Invoke();
    }

    public void TryTriggerMinigame()
    {
        _minigameManager.TryTriggerMinigame(TriggerDialogue);
    }
    
    public void TryLevelUp()
    {
        _levelManager.TryLevelUp(TriggerDialogue);
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
