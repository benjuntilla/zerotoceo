using UnityEngine;

namespace MinigameTrash
{
	public class Player : MonoBehaviour, IMinigamePlayer
	{
    	public float movementSpeed { get; set; }
    
    	void Update()
    	{
        	transform.Translate(Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime,0,0);
    	}
	}
}
