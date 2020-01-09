using UnityEngine;
using System.Collections;
using Prime31;

public class NPCController : MonoBehaviour
{
    public bool dialogueTriggered;

    public int intervalMin = 1;
    public int intervalMax = 4;
	public float gravity = -10f;
	public float runSpeed = 1f;
    public bool standStill = true;

    private CharacterMovement2D _characterMovement;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
    private GameObject _player;
    private InteractableController _interactableController;
    private GameObject _exclamation;
    private int _direction;
    private int _interval;
    private float _lastMoveTime;
    private bool _nearPlayer;
    private bool _followPlayer;
    private bool _wander = true;

    void Awake()
    {
        _animator = GetComponent<Animator>();
		_characterMovement = GetComponent<CharacterMovement2D>();
        _interactableController = GetComponent<InteractableController>();
        _player = GameObject.FindWithTag("Player");
        _exclamation = transform.Find("Exclamation").gameObject;
    }

    void Update()
    {
        _exclamation.SetActive(dialogueTriggered);

        if (dialogueTriggered || DialogueManager.CurrentDialogue == _interactableController.dialogue.name)
        {
            if (DialogueManager.CurrentDialogue != _interactableController.dialogue.name && _nearPlayer)
            {
                _interactableController.TriggerDialogue();
                dialogueTriggered = false;
            }
            _followPlayer = !standStill && !_nearPlayer;
            _wander = false;
        } else
        {
            _followPlayer = false;
            _wander = !standStill;
        }

        if (Mathf.RoundToInt(_player.transform.position.x) == Mathf.RoundToInt(transform.position.x))
            _nearPlayer = true;
        else
            _nearPlayer = false;

        if (_wander && Time.realtimeSinceStartup - _lastMoveTime > _interval)
        {
            _direction = Random.Range(-1, 2); // The second argument minus one is the range's maximum value
            _interval = Random.Range(intervalMin, intervalMax + 1);
            _lastMoveTime = Time.realtimeSinceStartup;
        } else if (_followPlayer && !_nearPlayer)
        {
            if (_player.transform.position.x > transform.position.x)
                _direction = 1;
            else if (_player.transform.position.x < transform.position.x)
                _direction = -1;
        } else if (!_wander)
        {
            _direction = 0;
            _interval = 0;
            _lastMoveTime = Time.realtimeSinceStartup;
        }

        if( _characterMovement.isGrounded )
                _velocity.y = 0;

        if( _direction == 1 )
        {
            if( transform.localScale.x < 0f && Time.timeScale == 1f)
                transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
            if( _characterMovement.isGrounded )
                _animator.Play( Animator.StringToHash( "Run" ) );
            if(_characterMovement.velocity.x != 1 && _wander)
                _direction = -1;
        }
        else if( _direction == -1 )
        {
            if( transform.localScale.x > 0f && Time.timeScale == 1f)
                transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
            if( _characterMovement.isGrounded )
                _animator.Play( Animator.StringToHash( "Run" ) );
            if(_characterMovement.velocity.x != -1 && _wander)
                _direction = 1;
        }
        else
        {
            if( _characterMovement.isGrounded )
                _animator.Play( Animator.StringToHash( "Idle" ) );
        }

        // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
        _velocity.x = _direction * runSpeed;

        // apply gravity before moving
        _velocity.y += gravity * Time.deltaTime;

        _characterMovement.move(_velocity * Time.deltaTime);

        // grab our current _velocity to use as a base for all calculations
		_velocity = _characterMovement.velocity;
    }
}
