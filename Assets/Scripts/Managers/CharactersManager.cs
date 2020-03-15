using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    private NPC _managerNPC;
    private Player _player;

    public GameObject indicatorTarget;

    void Awake()
    {
        _managerNPC = GameObject.Find("Manager").GetComponent<NPC>();
        _player = FindObjectOfType<Player>();
    }

    public void TriggerManagerDialogue()
    {
        _managerNPC.dialogueTriggered = true;
    }

    void Update()
    {
        indicatorTarget = _player.indicatorTarget;
    }
}
