using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
	public int points { get { return _points; } set { _points = value; } }
	public int lives { get { return _lives; } set { _lives = value <= 3 ? value : 3; } }

	[Header("Movement config")]
	public float movementSpeed = 1.75f, jumpHeight = 1f;
	public GameObject indicatorTarget;

	private static int _lives = 3, _points;
    private DialogueManager _dialogueManager;
    private InteractableController[] _interactables;
    private Dictionary<InteractableController, float> _interactablesDistances = new Dictionary<InteractableController, float>();
    private Animator _animator;
    private Rigidbody2D _rb;
    private bool _isGrounded;
    private float _inputAxisX, _inputAxisY;

    void Awake()
    {
	    _dialogueManager = FindObjectOfType<DialogueManager>();
        _interactables = FindObjectsOfType<InteractableController>();
        
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void UpdateIndicator()
    {
	    // Calculate distances from interactables and determine which is closest
	    foreach (var interactable in _interactables)
	    {
		    var distance = Vector3.Distance(interactable.transform.position, transform.position);
		    _interactablesDistances[interactable] = distance;
	    }
	    var closest = _interactablesDistances.OrderBy(k => k.Value).FirstOrDefault();

	    // Activate indicator & allow for interaction when the player is near an interactable
	    if (Mathf.RoundToInt(closest.Key.transform.position.x) == Mathf.RoundToInt(transform.position.x) && _dialogueManager.currentDialogue == "")
	    {
		    indicatorTarget = closest.Key.gameObject;
		    if (Input.GetButtonDown("Interact"))
			    closest.Key.Interact();
	    } else
	    {
		    indicatorTarget = null;
	    }
    }

    void OnCollisionStay2D()
    {
	    if (_rb.velocity.y == 0)
			_isGrounded = true;
    }

    void Update()
    {
	    UpdateIndicator();
	    _inputAxisX = Input.GetAxisRaw("Horizontal");
	    _inputAxisY = Input.GetAxisRaw("Vertical");
	    
	    // Only jump when grounded
	    if( (_isGrounded && _inputAxisY == 1) || (_isGrounded && Input.GetButtonDown("Jump")) )
	    {
		    _isGrounded = false;
		    _rb.AddForce(new Vector2(0.0f, 2.0f) * jumpHeight, ForceMode2D.Impulse);		
		    AudioManager.instance.PlayOnce("PlayerJump");
	    }
	    
	    // Flips sprite to the appropriate direction
	    if( _inputAxisX == 1 && transform.localScale.x < 0f && Time.timeScale == 1f)
			transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
	    else if( _inputAxisX == -1 && transform.localScale.x > 0f && Time.timeScale == 1f)
			transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
	    
	    // Plays the appropriate animation
	    _animator.Play(Animator.StringToHash(_inputAxisX != 0 && _isGrounded ? "Run" : _isGrounded ? "Idle" : "Jump"));
	    
	    // Plays sound when walking
	    if (_inputAxisX != 0 && _isGrounded && _rb.velocity.x != 0)
		    AudioManager.instance.PlayOnce("PlayerWalk");
	    else
		    AudioManager.instance.Stop("PlayerWalk");
    }

    void FixedUpdate()
    {
	    // Moves the player
	    _rb.velocity = new Vector2(_inputAxisX * movementSpeed, _rb.velocity.y);
    }
}
