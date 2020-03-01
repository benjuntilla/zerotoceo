using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    private NPCController _managerNPCController;
    private PlayerController _playerController;

    public GameObject indicatorTarget;

    void Awake()
    {
        _managerNPCController = GameObject.Find("Manager").GetComponent<NPCController>();
        _playerController = FindObjectOfType<PlayerController>();
    }

    public void TriggerManagerDialogue()
    {
        _managerNPCController.dialogueTriggered = true;
    }

    void Update()
    {
        indicatorTarget = _playerController.indicatorTarget;
    }
}
