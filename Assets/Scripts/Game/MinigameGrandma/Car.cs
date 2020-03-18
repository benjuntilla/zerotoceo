using System.Linq;
using Random = UnityEngine.Random;
using UnityEngine;

namespace MinigameGrandma
{
    [RequireComponent(typeof(Collidable))]
    public class Car : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Rigidbody2D _rigidBody2D;
        private Collidable _collidable;
        private MinigameGrandmaManager _minigameGrandmaManager;
        private MinigamePlayer _minigamePlayer;
        private MinigameManager _minigameManager;
        private GameObject _topDestroy, _bottomDestroy;
        private bool _stopped;

        [HideInInspector] public int direction;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _collidable = GetComponent<Collidable>();
            DetermineDirection();
            CheckIfFlip();
        }
        

        private void Start()
        {
            _minigamePlayer = FindObjectOfType<MinigamePlayer>();
            _minigameGrandmaManager = FindObjectOfType<MinigameGrandmaManager>();
            _minigameManager = FindObjectOfType<MinigameManager>();
            _topDestroy = GameObject.FindWithTag("World").transform.Find("Car Destroy Areas").transform.Find("Top").gameObject;
            _bottomDestroy = GameObject.FindWithTag("World").transform.Find("Car Destroy Areas").transform.Find("Bottom").gameObject;
            SetUpCollidable();
            RandomizeSprite();
        }

        private void SetUpCollidable()
        {
            _collidable.primaryCollisionObjects.Add(_minigamePlayer.gameObject);
            _collidable.tertiaryCollisionObjects.Add(_topDestroy);
            _collidable.tertiaryCollisionObjects.Add(_bottomDestroy);
            
            _collidable.primaryCollisionEvent.AddListener(HitPlayer);
            _collidable.secondaryCollisionEvent.AddListener(StopSelf);
            _collidable.tertiaryCollisionEvent.AddListener(DestroySelf);
        }

        private void DetermineDirection()
        {
            direction = (gameObject.transform.position.y > 0) ? -1 : 1;
        }

        private void CheckIfFlip()
        {
            transform.localScale = new Vector3(transform.localScale.x,
                direction < 0 ? transform.localScale.y : -transform.localScale.y, transform.localScale.z);
        }

        private void RandomizeSprite()
        {
            _renderer.sprite = _minigameGrandmaManager.carSprites[Random.Range(0, _minigameGrandmaManager.carSprites.Count)];
        }

        private void HitPlayer()
        {
            _minigameManager.Fail();
            _minigamePlayer.OnMinigameFail();
            Invoke(nameof(StopSelf), 0.5f);
        }

        private void DestroySelf()
        {
            _minigameGrandmaManager.cars.Remove(gameObject);
            Destroy(gameObject);
        }

        private void StopSelf()
        { 
            _stopped = true;
            _rigidBody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void StartSelf()
        {
            _stopped = false;
            _rigidBody2D.constraints = RigidbodyConstraints2D.None;
        }

        private void UpdateCollisionObjects()
        {
            _collidable.secondaryCollisionObjects = _minigameGrandmaManager.carTriggers;

            _collidable.tertiaryCollisionObjects = _collidable.tertiaryCollisionObjects.Except(_minigameGrandmaManager.cars).ToList();
            _collidable.tertiaryCollisionObjects.AddRange(_minigameGrandmaManager.cars);
        }

        private void Update()
        {
            UpdateCollisionObjects();
        }

        private void FixedUpdate()
        {
            _rigidBody2D.velocity = !_stopped ? (Vector2)transform.up * (direction * _minigameGrandmaManager.carMovementSpeed) : Vector2.zero;
            if (!_collidable.insideATrigger && _stopped) StartSelf();
        }
    }
}