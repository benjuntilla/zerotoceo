using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))] // Collisions require rigidbody
public class CollidableController : MonoBehaviour
{
    public UnityEvent collisionMethod, secondaryCollisionMethod;
    public GameObject targetObject, secondaryTargetObject;
    public List<GameObject> targetObjectList;
    public LayerMask targetLayer;
    public bool collisionEventsEnabled = true;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!collisionEventsEnabled) return;
        if (targetObject != null && collision.gameObject == targetObject)
            collisionMethod.Invoke();
        else if (targetObjectList.Contains(collision.gameObject))
            collisionMethod.Invoke();
        else if ((targetLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            collisionMethod.Invoke();
        else if (secondaryTargetObject != null && collision.gameObject == secondaryTargetObject)
            secondaryCollisionMethod.Invoke();
        else if (targetObject == null && secondaryTargetObject == null)
            collisionMethod.Invoke();
    }
}