using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    private NPCController _managerNPCController;

    public GameObject indicatorTarget;

    void Awake()
    {
        _managerNPCController = GameObject.Find("Manager").GetComponent<NPCController>();
    }

    public void TriggerManagerDialogue()
    {
        _managerNPCController.dialogueTriggered = true;
    }
}
