using System;
using UnityEngine;

public abstract class MinigamePlayer : MonoBehaviour
{
    public float movementSpeed;
    public bool blockInput = true;

    protected Rigidbody2D rigidBody;
    protected Collider2D playerCollider;
    protected Vector2 input;
    
    private MinigameManager _minigameManager;

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    protected virtual void Start()
    {
        _minigameManager = FindObjectOfType<MinigameManager>();
    }

    protected virtual void OnUpdate() {}

    protected virtual void Move() {}

    public virtual void OnMinigameFail() {}
    
    public virtual void OnMinigamePass() {}

    private void Update()
    {
        if (MinigameManager.minigameInfo.status == MinigameManager.Status.InProgress && !blockInput)
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        else
            input = new Vector2(0, 0);
        OnUpdate();
    }

    private void FixedUpdate()
    {
        Move();
    }
}
