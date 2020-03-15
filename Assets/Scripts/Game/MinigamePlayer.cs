using UnityEngine;

public abstract class MinigamePlayer : MonoBehaviour
{
    public float movementSpeed;
    public bool blockInput = true;

    protected Rigidbody2D rigidBody;
    protected Vector2 input;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    protected virtual void OnUpdate() {}

    protected virtual void Move() {}

    private void Update()
    {
        if (MinigameManager.minigameStatus == MinigameManager.Status.InProgress && !blockInput)
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
