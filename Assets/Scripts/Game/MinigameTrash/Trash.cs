using UnityEngine;

[RequireComponent(typeof(Collidable), typeof(Draggable))]
public class Trash : MonoBehaviour
{
    private MinigameManager _minigameManager;
    private MinigameTrashManager _minigameTrashManager;
    private Rigidbody2D _rigidbody2D;
    private Collidable _collidable;
    
    void Awake()
    {
        var gameManagers = GameObject.FindWithTag("GameManagers");
        _minigameManager = gameManagers.GetComponent<MinigameManager>();
        _minigameTrashManager = gameManagers.GetComponent<MinigameTrashManager>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collidable = GetComponent<Collidable>();
        
        _collidable.primaryCollisionEvent.AddListener(_minigameTrashManager.TestFail);
        _collidable.primaryCollisionEvent.AddListener(FreezePosition);
        _collidable.primaryCollisionEvent.AddListener(_collidable.DisableCollisionEvents);
        _collidable.secondaryCollisionEvent.AddListener(delegate{Destroy(gameObject);});
        
        _collidable.primaryCollisionObjects.Add(GameObject.FindWithTag("World").transform.Find("Ground").gameObject);
        _collidable.secondaryCollisionObjects.Add(GameObject.FindWithTag("Player").gameObject);

        ApplyGameConfig();
    }
    private void ApplyGameConfig()
    {
        _rigidbody2D.gravityScale = _minigameTrashManager.trashGravity;
        _rigidbody2D.AddTorque(_minigameTrashManager.trashTorque, ForceMode2D.Impulse);
    }

    private void FreezePosition()
    {
        _rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
        _rigidbody2D.freezeRotation = true;
    } 
}
