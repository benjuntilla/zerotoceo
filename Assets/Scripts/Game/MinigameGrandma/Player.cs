using DG.Tweening;
using UnityEngine;

namespace MinigameGrandma
{
    public class Player : MinigamePlayer
    {
        private void Start()
        {
            movementSpeed = FindObjectOfType<MinigameGrandmaManager>().playerMovementSpeed;
        }

        protected override void Move()
        {
            rigidBody.velocity = new Vector2(input.x, input.y) * movementSpeed;
        }

        protected override void OnUpdate()
        {
            if (input.x == 1f && transform.localScale.x < 0f && Time.timeScale == 1f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            if (input.x == -1f && transform.localScale.x > 0f && Time.timeScale == 1f)
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        public override void OnMinigamePass()
        {
            playerCollider.enabled = false;
            transform.DOMoveX(transform.position.x + 2, 2f);
        }

        public override void OnMinigameFail()
        {
            rigidBody.constraints = RigidbodyConstraints2D.None;
        }
    }
}
