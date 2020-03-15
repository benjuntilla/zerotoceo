using UnityEngine;

namespace MinigameTrash
{
	public class Player : MinigamePlayer
	{
		protected override void Move()
		{
			rigidBody.velocity = new Vector2(input.x * movementSpeed, 0);
		}
	}
}
