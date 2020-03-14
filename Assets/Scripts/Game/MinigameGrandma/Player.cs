using UnityEngine;

namespace MinigameGrandma
{
    public class Player : MonoBehaviour, IMinigamePlayer
    {
        public float movementSpeed { get; set; }
    
        void Update()
        {
            if (Input.GetAxisRaw("Horizontal") == 1)
            {
                transform.Translate(Vector3.right * Time.deltaTime * movementSpeed);
                if (transform.localScale.x < 0f && Time.timeScale == 1f)
                    transform.localScale =
                        new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
	
            if (Input.GetAxisRaw("Horizontal") == -1)
            {
                transform.Translate(Vector3.left * Time.deltaTime * movementSpeed);
                if (transform.localScale.x > 0f && Time.timeScale == 1f)
                    transform.localScale =
                        new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }

            if (Input.GetAxisRaw("Vertical") == 1)
            {
                transform.Translate(Vector3.up * Time.deltaTime * movementSpeed);
            }
	
            if (Input.GetAxisRaw("Vertical") == -1)
            {
                transform.Translate(Vector3.down * Time.deltaTime * movementSpeed);
            }
        }
    }
}
