using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MinigameGrandma
{
    public class MinigameGrandmaManager : Minigame
	{
		private GameObject _world, _roads, _characters;
		private Vector2[] _roadSpawnVectors;

		[HideInInspector] public float carWaitTime, carMovementSpeed, playerMovementSpeed;
		[HideInInspector] public List<GameObject> carArray = new List<GameObject>();
		[Header("Game Config")] 
		public float maxRandomSpawnTime;
		public GameObject carPrefab;
		public List<Sprite> carSprites;
		
		#region public config classes
		[System.Serializable]
		public class EasyDifficultyConfig
		{
			public float playerMovementSpeed = 4;
			public float carMovementSpeed = 4;
			public float carWaitTime = 1f;
		}
		public EasyDifficultyConfig easyDifficultyConfig;
		
		[System.Serializable]
		public class MediumDifficultyConfig
		{
			public float playerMovementSpeed = 4;
			public float carMovementSpeed = 4;
			public float carWaitTime = 1f;
		}
		public MediumDifficultyConfig mediumDifficultyConfig;
		
		[System.Serializable]
		public class HardDifficultyConfig
		{
			public float playerMovementSpeed = 4;
			public float carMovementSpeed = 4;
			public float carWaitTime = 1f;
		}
		public HardDifficultyConfig hardDifficultyConfig;
		#endregion

		void Start()
		{
			_characters = GameObject.Find("Characters");
			_world = GameObject.FindWithTag("World");
			_roads = _world.transform.Find("Roads").gameObject;
			
			// Randomize road spawn points
			_roadSpawnVectors = new Vector2[_roads.transform.childCount];
			for (var i = 0; i < _roads.transform.childCount; i++)
			{
				if (Random.Range(0, 2) == 0)
					_roadSpawnVectors[i] = new Vector2(_roads.transform.GetChild(i).position.x, 10);
				else
					_roadSpawnVectors[i] = _roadSpawnVectors[i] = new Vector2(_roads.transform.GetChild(i).position.x, -10); 
			}
			
			LoadDifficultyConfig();
		}

		private void LoadDifficultyConfig()
		{
			switch (MinigameManager.minigameDifficulty)
			{
				case "Hard":
					playerMovementSpeed = hardDifficultyConfig.playerMovementSpeed;
					carMovementSpeed = hardDifficultyConfig.carMovementSpeed;
					carWaitTime = hardDifficultyConfig.carWaitTime;
					break;
				case "Medium":
					playerMovementSpeed = mediumDifficultyConfig.playerMovementSpeed;
					carMovementSpeed = mediumDifficultyConfig.carMovementSpeed;
					carWaitTime = mediumDifficultyConfig.carWaitTime;
					break;
				default: // "Easy"
					playerMovementSpeed = easyDifficultyConfig.playerMovementSpeed;
					carMovementSpeed = easyDifficultyConfig.carMovementSpeed;
					carWaitTime = easyDifficultyConfig.carWaitTime;
					break;
			}
			
			minigamePlayer.movementSpeed = playerMovementSpeed;
		}

		public override void OnMinigameStart()
		{
			base.OnMinigameStart();
			StartCoroutine(CarSpawnLoop());
		}

		private IEnumerator CarSpawnLoop()
		{
			while (true)
			{
				for (var i = 0; i < _roads.transform.childCount; i++)
				{
					StartCoroutine(SpawnCar(_roadSpawnVectors[i]));
				}
				yield return new WaitForSeconds(carWaitTime);
			}
		}

		private IEnumerator SpawnCar(Vector2 position)
		{
			// Wait for a short, arbitrary duration to make the cars more random 
			yield return new WaitForSeconds(Random.Range(0f, maxRandomSpawnTime));

			var car = Instantiate(carPrefab, position, Quaternion.identity);
			car.transform.parent = _characters.transform;
			carArray.Add(car);
			if (position.y > 0)
				car.GetComponent<Car>().direction = -1;
			else
				car.GetComponent<Car>().direction = 1;
		}
	}
}
