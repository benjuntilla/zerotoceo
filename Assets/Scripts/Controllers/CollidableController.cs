using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))] // Collisions require a Rigidbody
public class CollidableController : MonoBehaviour
{
    public List<GameObject> primaryCollisionObjects = new List<GameObject>(), secondaryCollisionObjects = new List<GameObject>();
    public UnityEvent primaryCollisionEvent = new UnityEvent(), secondaryCollisionEvent = new UnityEvent();
    
    private bool _collisionEventsEnabled = true;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!_collisionEventsEnabled) return;
        if (primaryCollisionObjects.Contains(collision.gameObject))
            primaryCollisionEvent.Invoke();
        if (secondaryCollisionObjects.Contains(collision.gameObject))
            secondaryCollisionEvent.Invoke();
    }

    public void EnableCollisionEvents()
    {
        _collisionEventsEnabled = true;
    }
    
    public void DisableCollisionEvents()
    {
        _collisionEventsEnabled = false;
    }
}