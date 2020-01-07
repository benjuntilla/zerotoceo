using Prime31;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	public bool godMode;
	public static int Points;
	public static int Lives = 3;
	[Header("Movement config")]
    public float gravity = -10f;
    public float runSpeed = 2f;
    public float groundDamping = 99f; // how fast do we change direction? higher means faster
    public float inAirDamping = 99f;
    public float jumpHeight = 1f;
    public float fallThreshold = -5f;
    public float jumpThreshold = 1f;
    public Vector3 velocity;
    
    private InteractableController[] _interactables;
    private Dictionary<InteractableController, float> _interactablesDistances = new Dictionary<InteractableController, float>();
    private GameObject _indicatorUI;
    private float _normalizedHorizontalSpeed;
    private CharacterMovement2D _characterMovement;
    private Animator _animator;

    void Awake()
    {
        _interactables = GameObject.FindObjectsOfType<InteractableController>();
        _indicatorUI = GameObject.Find("Indicator");
        
        _animator = GetComponent<Animator>();
        _characterMovement = GetComponent<CharacterMovement2D>();
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
	    if (Mathf.RoundToInt(closest.Key.transform.position.x) == Mathf.RoundToInt(transform.position.x) && DialogueManager.CurrentDialogue == "")
	    {
		    _indicatorUI.SetActive(true);
		    var pos = closest.Key.transform.position;
		    _indicatorUI.transform.position = new Vector3(pos.x, pos.y + .9f, 0);
		    if (Input.GetButtonDown("Interact"))
		    {
			    closest.Key.GetComponent<InteractableController>().Interact();
		    }
	    } else
	    {
		    _indicatorUI.SetActive(false);
	    }
    }

    void Update()
    {
	    UpdateIndicator();
        
        #region player movement
        
	    if( _characterMovement.isGrounded )
			velocity.y = 0;

		if( Input.GetAxisRaw("Horizontal") == 1 )
		{
			_normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f && Time.timeScale == 1f)
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _characterMovement.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else if( Input.GetAxisRaw("Horizontal") == -1 )
		{
			_normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f && Time.timeScale == 1f)
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _characterMovement.isGrounded )
				_animator.Play( Animator.StringToHash( "Run" ) );
		}
		else
		{
			_normalizedHorizontalSpeed = 0;

			if( _characterMovement.isGrounded )
				_animator.Play( Animator.StringToHash( "Idle" ) );
		}

		// we can only jump whilst grounded
		if( _characterMovement.isGrounded && Input.GetAxisRaw("Vertical") == 1 )
		{
			velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
		}
		if( _characterMovement.isGrounded && Input.GetButtonDown("Jump") )
		{
			velocity.y = Mathf.Sqrt( 2f * jumpHeight * -gravity );
		}

		// play falling animation when visibly moving downwards
		if( !_characterMovement.isGrounded && velocity.y < fallThreshold)
		{
			_animator.Play( Animator.StringToHash( "Fall" ) );
		}

		// play jumping animation when visibly moving upwards
		if( !_characterMovement.isGrounded && velocity.y > jumpThreshold)
		{
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}

		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _characterMovement.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		velocity.x = Mathf.Lerp( velocity.x, _normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
		if( _characterMovement.isGrounded && Input.GetAxisRaw("Vertical") == -1 )
		{
			velocity.y *= 3f;
			_characterMovement.ignoreOneWayPlatformsThisFrame = true;
		}

		_characterMovement.move( velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		velocity = _characterMovement.velocity;
		
		#endregion
		
		if (!godMode) return;
		Points = 9999;
		Lives = 9999;
    }
}
