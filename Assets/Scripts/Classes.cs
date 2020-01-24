using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CollidableController), typeof(DraggableController))]
public abstract class Coin : MonoBehaviour
{
    protected MinigameManager minigameManager;
    protected MinigameCoinManager minigameCoinManager;
    protected DraggableController draggableController;
    protected CollidableController collidableController;
    protected GameObject pennyRegion, nickelRegion, dimeRegion, quarterRegion;
    
    void Awake()
    {
        var gameManagers = GameObject.FindWithTag("GameManagers");
        minigameManager = gameManagers.GetComponent<MinigameManager>();
        minigameCoinManager = gameManagers.GetComponent<MinigameCoinManager>();
        draggableController = GetComponent<DraggableController>();
        collidableController = GetComponent<CollidableController>();
        
        collidableController.primaryCollisionEvent.AddListener(delegate { Destroy(gameObject); });
        collidableController.secondaryCollisionEvent.AddListener(collidableController.DisableCollisionEvents);
        collidableController.secondaryCollisionEvent.AddListener(minigameManager.Fail);
        draggableController.dragEvent.AddListener(collidableController.DisableCollisionEvents);
        draggableController.dropEvent.AddListener(collidableController.EnableCollisionEvents);
        
        var dropRegions = GameObject.FindWithTag("World").transform.Find("Drop Regions").gameObject;
        pennyRegion = dropRegions.transform.Find("Penny Region").gameObject;
        nickelRegion = dropRegions.transform.Find("Nickel Region").gameObject;
        dimeRegion = dropRegions.transform.Find("Dime Region").gameObject;
        quarterRegion = dropRegions.transform.Find("Quarter Region").gameObject;

        ApplyCollisionObjects();
    }

    protected abstract void ApplyCollisionObjects();
}

public interface IMinigamePlayer
{
    float movementSpeed { get; set; }
}

public interface IMinigameManager
{
    bool countDownNecessary { get; set; }
    void StartGame();
}
