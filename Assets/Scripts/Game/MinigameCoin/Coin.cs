using UnityEngine;

[RequireComponent(typeof(Collidable), typeof(Draggable))]
public abstract class Coin : MonoBehaviour
{
    protected MinigameManager minigameManager;
    protected Draggable draggable;
    protected Collidable collidable;
    protected GameObject pennyRegion, nickelRegion, dimeRegion, quarterRegion;
    
    void Start()
    {
        var gameManagers = GameObject.FindWithTag("GameManagers");
        minigameManager = gameManagers.GetComponent<MinigameManager>();
        draggable = GetComponent<Draggable>();
        collidable = GetComponent<Collidable>();
        
        collidable.primaryCollisionEvent.AddListener(delegate { Destroy(gameObject); });
        collidable.secondaryCollisionEvent.AddListener(collidable.DisableCollisionEvents);
        collidable.secondaryCollisionEvent.AddListener(minigameManager.Fail);
        draggable.dragEvent.AddListener(collidable.DisableCollisionEvents);
        draggable.dropEvent.AddListener(collidable.EnableCollisionEvents);
        
        var dropRegions = GameObject.FindWithTag("World").transform.Find("Drop Regions").gameObject;
        pennyRegion = dropRegions.transform.Find("Penny Region").gameObject;
        nickelRegion = dropRegions.transform.Find("Nickel Region").gameObject;
        dimeRegion = dropRegions.transform.Find("Dime Region").gameObject;
        quarterRegion = dropRegions.transform.Find("Quarter Region").gameObject;

        ApplyCollisionObjects();
    }

    protected abstract void ApplyCollisionObjects();
}
