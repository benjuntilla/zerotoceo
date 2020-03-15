using System;
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
        private float _time;
        private GameObject _topDestroy, _bottomDestroy;
        private bool _stopped;

        [HideInInspector] public int direction;
        public float stopThreshold;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _collidable = GetComponent<Collidable>();
        }

        private void Start()
        {
            _minigamePlayer = FindObjectOfType<MinigamePlayer>();
            _minigameGrandmaManager = FindObjectOfType<MinigameGrandmaManager>();
            _minigameManager = FindObjectOfType<MinigameManager>();
            _topDestroy = GameObject.FindWithTag("World").transform.Find("Car Destroy Areas").transform.Find("Top").gameObject;
            _bottomDestroy = GameObject.FindWithTag("World").transform.Find("Car Destroy Areas").transform.Find("Bottom").gameObject;
            
            _collidable.primaryCollisionObjects.Add(_minigamePlayer.gameObject);
            _collidable.primaryCollisionEvent.AddListener(_minigameManager.Fail);
            _collidable.primaryCollisionEvent.AddListener(() => { _stopped = true;});
            _collidable.secondaryCollisionObjects.Add(_topDestroy);
            _collidable.secondaryCollisionObjects.Add(_bottomDestroy);
            _collidable.secondaryCollisionEvent.AddListener(DestroySelf);
            
            _renderer.sprite = _minigameGrandmaManager.carSprites[Random.Range(0, _minigameGrandmaManager.carSprites.Count)];

            transform.localScale = new Vector3(transform.localScale.x,
                direction < 0 ? transform.localScale.y : -transform.localScale.y, transform.localScale.z);
        }

        private void DestroySelf()
        {
            _minigameGrandmaManager.carArray.Remove(gameObject);
            Destroy(gameObject);
        }

        private void Update()
        {
            foreach (GameObject car in _minigameGrandmaManager.carArray)
                if (Math.Abs(car.transform.position.y - transform.position.y) < stopThreshold && car != gameObject && car.transform.position.x == transform.position.x)
                    _stopped = true;        
        }

        private void FixedUpdate()
        {
            _rigidBody2D.velocity = !_stopped ? new Vector2(0, direction * _minigameGrandmaManager.carMovementSpeed) : Vector2.zero;
        }
    }
}