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
        minigameManager = FindObjectOfType<MinigameManager>();
        draggable = GetComponent<Draggable>();
        collidable = GetComponent<Collidable>();
        
        collidable.primaryCollisionEvent.AddListener(delegate { Destroy(gameObject); });
        collidable.secondaryCollisionEvent.AddListener(collidable.DisableCollisionEvents);
        collidable.secondaryCollisionEvent.AddListener(minigameManager.Fail);
        draggable.dragEvent.AddListener(collidable.DisableCollisionEvents);
        draggable.dropEvent.AddListener(collidable.EnableCollisionEvents);
        
        pennyRegion = GameObject.Find("Penny Region").gameObject;
        nickelRegion = GameObject.Find("Nickel Region").gameObject;
        dimeRegion = GameObject.Find("Dime Region").gameObject;
        quarterRegion = GameObject.Find("Quarter Region").gameObject;

        ApplyCollisionObjects();
    }

    protected abstract void ApplyCollisionObjects();
}
