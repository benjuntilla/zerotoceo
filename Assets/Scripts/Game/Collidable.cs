using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class Collidable : MonoBehaviour
{
    public List<GameObject> primaryCollisionObjects = new List<GameObject>(), secondaryCollisionObjects = new List<GameObject>(), tertiaryCollisionObjects = new List<GameObject>();
    public UnityEvent primaryCollisionEvent = new UnityEvent(), secondaryCollisionEvent = new UnityEvent(), tertiaryCollisionEvent = new UnityEvent();
    public bool insideATrigger;

    private int _triggerCount;
    private bool _collisionEventsEnabled = true;
    private List<GameObject> _triggeredObjectList;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _triggerCount++;
        insideATrigger = true;
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_collisionEventsEnabled) return;
        if (primaryCollisionObjects.Contains(collision.gameObject))
            primaryCollisionEvent.Invoke();
        else if (secondaryCollisionObjects.Contains(collision.gameObject))
            secondaryCollisionEvent.Invoke();
        else if (tertiaryCollisionObjects.Contains(collision.gameObject))
            tertiaryCollisionEvent.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _triggerCount--;
        if (_triggerCount == 0) insideATrigger = false;
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