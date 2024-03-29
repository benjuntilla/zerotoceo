﻿using System.Collections.Generic;
using System.Linq;
using UI;
using UnityEngine;

public class Player : MonoBehaviour
{
	public int points;
	public int lives { get { return _lives; } set { _lives = value <= 3 ? value : 3; } }

	[Header("Movement config")]
	public float movementSpeed = 1.75f, jumpHeight = 1f;
	public GameObject indicatorTarget;

	private int _lives = 3;
    private DialogueManager _dialogueManager;
    private Interactable[] _interactables;
    private Dictionary<Interactable, float> _interactablesDistances = new Dictionary<Interactable, float>();
    private Animator _animator;
    private Rigidbody2D _rb;
    private MenuFull _menuFull;
    private bool _isGrounded;
    private float _inputAxisX, _inputAxisY;

    void Awake()
    {
	    _animator = GetComponent<Animator>();
	    _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
	    _dialogueManager = FindObjectOfType<DialogueManager>();
        _interactables = FindObjectsOfType<Interactable>();
        _menuFull = FindObjectOfType<MenuFull>();
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
	    if (Mathf.RoundToInt(closest.Key.transform.position.x) == Mathf.RoundToInt(transform.position.x) && _dialogueManager.currentDialogueName == "")
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
	    _inputAxisX = Time.timeScale == 1f ? Input.GetAxisRaw("Horizontal") : 0;
	    _inputAxisY = Time.timeScale == 1f ? Input.GetAxisRaw("Vertical") : 0;
	    
	    // Only jump when grounded
	    if( (_isGrounded && _inputAxisY == 1) || (_isGrounded && Input.GetButtonDown("Jump")) )
	    {
		    _isGrounded = false;
		    _rb.AddForce(new Vector2(0.0f, 2.0f) * jumpHeight, ForceMode2D.Impulse);		
		    AudioManager.instance.PlayOnce("PlayerJump");
	    }
	    
	    // Flips sprite to the appropriate direction
	    if( _inputAxisX == 1 && transform.localScale.x < 0f)
			transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
	    else if( _inputAxisX == -1 && transform.localScale.x > 0f)
			transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
	    
	    // Plays the appropriate animation
	    _animator.Play(Animator.StringToHash(_inputAxisX != 0 && _isGrounded ? "Run" : _isGrounded ? "Idle" : "Jump"));
	    
	    // Plays sound when walking
	    if (_inputAxisX != 0 && _isGrounded && _rb.velocity.x != 0)
		    AudioManager.instance.PlayOnce("PlayerWalk");
	    else
		    AudioManager.instance.Stop("PlayerWalk");
	    
	    // Triggers death menu when appropriate
	    if (_lives == 0)
		    _menuFull.Trigger("death");
    }

    void FixedUpdate()
    {
	    // Moves the player
	    _rb.velocity = new Vector2(_inputAxisX * movementSpeed, _rb.velocity.y);
    }
}
