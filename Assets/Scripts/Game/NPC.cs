using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NPC : MonoBehaviour
{
    public bool dialogueTriggered;

    [Header("Movement config")]
    public int wanderIntervalMin = 1;
    public int wanderIntervalMax = 4;
	public float runSpeed = 1f;
    public bool standStill = true;
    public float proximityThreshold = 0.75f;

    private DialogueManager _dialogueManager;
	private Animator _animator;
    private Rigidbody2D _rb;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
    private GameObject _player, _exclamation, _dots;
    private Interactable _interactable;
    private int _interval;
    private float _lastMoveTime;
    private bool _wander = true, _isGrounded, _followPlayer, _nearPlayer;

    void Start()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _interactable = GetComponent<Interactable>();
        _player = GameObject.FindWithTag("Player");
        _exclamation = transform.Find("Exclamation").gameObject;
        _dots = transform.Find("Dots").gameObject;
    }

    private void OnCollisionStay2D()
    {
        _isGrounded = true;
    }

    void Update()
    {
        // Set indicators based on certain conditions
        _exclamation.SetActive(dialogueTriggered);
        _dots.SetActive(_dialogueManager.currentDialogueName == _interactable.dialogue.name);

        // Controls when the NPC follows the player, stands still, or wanders depending on the current dialogue status
        if (dialogueTriggered || _dialogueManager.currentDialogueName == _interactable.dialogue.name)
        {
            if (_dialogueManager.currentDialogueName != _interactable.dialogue.name && _nearPlayer)
            {
                _interactable.TriggerDialogue();
                dialogueTriggered = false;
            }
            _followPlayer = !standStill && !_nearPlayer;
            _wander = false;
        } else
        {
            _followPlayer = false;
            _wander = !standStill;
        }

        // Determines whether the NPC is near the player
        _nearPlayer = Math.Abs(_player.transform.position.x - transform.position.x) < proximityThreshold;

        // Randomly sets the movement direction when wandering
        if (_wander && Time.realtimeSinceStartup - _lastMoveTime > _interval)
        {
            _velocity.x = Random.Range(-1, 2); // The second argument minus one is the range's maximum value
            _interval = Random.Range(wanderIntervalMin, wanderIntervalMax + 1);
            _lastMoveTime = Time.realtimeSinceStartup;
        } 
        // Sets the movement direction when following the player
        else if (_followPlayer && !_nearPlayer)
        {
            if (_player.transform.position.x > transform.position.x)
                _velocity.x = 1;
            else if (_player.transform.position.x < transform.position.x)
                _velocity.x = -1;
        } 
        // Stands still when neither wandering nor following the player
        else if (!_wander)
        {
            _velocity.x = 0;
            _interval = 0;
            _lastMoveTime = Time.realtimeSinceStartup;
        }
        
        // Looks at the player when having a dialogue
        if (_dialogueManager.currentDialogueName == _interactable.dialogue.name && _player.transform.position.x > transform.position.x && transform.localScale.x < 0f && Time.timeScale == 1f)
            transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
        else if (_dialogueManager.currentDialogueName == _interactable.dialogue.name && _player.transform.position.x < transform.position.x && transform.localScale.x > 0f && Time.timeScale == 1f)
            transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
        
        // Plays the appropriate animation
        _animator.Play(Animator.StringToHash(_velocity.x != 0 && _isGrounded ? "Run" : "Idle"));
        
        // Faces the sprite to the appropriate direction
        if( _velocity.x == 1 && transform.localScale.x < 0f && Time.timeScale == 1f)
            transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
        else if( _velocity.x == -1 && transform.localScale.x > 0f && Time.timeScale == 1f)
            transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

        // Flips direction if running into a wall
        if (_rb.velocity.x == 0 && _wander)
            _velocity.x *= -1;
    }

    private void FixedUpdate()
    {
        // Moves the NPC
        _rb.velocity = new Vector2(_velocity.x * runSpeed, _rb.velocity.y);
    }
}
