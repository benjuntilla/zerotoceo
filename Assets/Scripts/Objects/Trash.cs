using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollidableController), typeof(DraggableController))]
public class Trash : MonoBehaviour
{
    private MinigameManager _minigameManager;
    private MinigameTrashManager _minigameTrashManager;
    private Rigidbody2D _rigidbody2D;
    private CollidableController _collidableController;
    
    void Awake()
    {
        var gameManagers = GameObject.FindWithTag("GameManagers");
        _minigameManager = gameManagers.GetComponent<MinigameManager>();
        _minigameTrashManager = gameManagers.GetComponent<MinigameTrashManager>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collidableController = GetComponent<CollidableController>();
        
        _collidableController.primaryCollisionEvent.AddListener(_minigameTrashManager.TestFail);
        _collidableController.primaryCollisionEvent.AddListener(FreezePosition);
        _collidableController.primaryCollisionEvent.AddListener(_collidableController.DisableCollisionEvents);
        _collidableController.secondaryCollisionEvent.AddListener(delegate{Destroy(gameObject);});
        
        _collidableController.primaryCollisionObjects.Add(GameObject.FindWithTag("World").transform.Find("Ground").gameObject);
        _collidableController.secondaryCollisionObjects.Add(GameObject.FindWithTag("Player").gameObject);

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
