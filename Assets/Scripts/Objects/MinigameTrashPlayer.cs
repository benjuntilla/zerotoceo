using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTrashPlayer : MonoBehaviour, IMinigamePlayer
{
    public float movementSpeed { get; set; }
    
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime,0,0);
    }
}
