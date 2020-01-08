using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using Prime31;

[RequireComponent(typeof(Rigidbody2D), typeof(CharacterMovement2D))] // Collisions require rigidbody & collidables require CharacterMovement2D to make collision events work
public class CollidableController : MonoBehaviour
{
    public UnityEvent collisionMethod, secondaryCollisionMethod;
    public GameObject targetObject, secondaryTargetObject;
    public List<GameObject> targetObjectList;
    public LayerMask targetLayer;
    public bool collisionEventsEnabled = true;

    private CharacterMovement2D _characterMovement;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement2D>();
        _characterMovement.onTriggerStayEvent += Collision;
    }

    void Collision(Collider2D collision)
    {
        if (!collisionEventsEnabled) return;
        if (targetObject != null && collision.gameObject == targetObject)
            collisionMethod.Invoke();
        if (targetObjectList.Contains(collision.gameObject))
            collisionMethod.Invoke();
        if ((targetLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            collisionMethod.Invoke();
        if (secondaryTargetObject != null && collision.gameObject == secondaryTargetObject)
            secondaryCollisionMethod.Invoke();
        if (targetObject == null && secondaryTargetObject == null)
            collisionMethod.Invoke();
    }
}