using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MinigameGrandma
{
    public class MinigameGrandmaManager : Minigame
	{
		private GameObject _world, _roads, _characters;
		private Vector2[] _roadSpawnVectors;

		[Header("Public fields")]
		public float carSpawnTime;
		public float carMovementSpeed;
		public float playerMovementSpeed;
		public List<GameObject> cars = new List<GameObject>(), carTriggers = new List<GameObject>();
		[Header("Game Config")] 
		public float carSpeedMultiplier = 4f;
		public float carSpawnMultiplier = 0.25f;
		public float maxSpawnFluctuationPercentage = 0.25f;
		public GameObject carPrefab;
		public List<Sprite> carSprites;
		#region public config classes
		[System.Serializable]
		public class EasyDifficultyConfig
		{
			public float playerMovementSpeed = 4;
			public float carMovementSpeed = 4;
			public float carSpawnTime = 1f;
		}
		public EasyDifficultyConfig easyDifficultyConfig;
		
		[System.Serializable]
		public class MediumDifficultyConfig
		{
			public float playerMovementSpeed = 4;
			public float carMovementSpeed = 4;
			public float carSpawnTime = 1f;
		}
		public MediumDifficultyConfig mediumDifficultyConfig;
		
		[System.Serializable]
		public class HardDifficultyConfig
		{
			public float playerMovementSpeed = 4;
			public float carMovementSpeed = 4;
			public float carSpawnTime = 1f;
		}
		public HardDifficultyConfig hardDifficultyConfig;
		#endregion

		void Start()
		{
			_characters = GameObject.Find("Characters");
			_world = GameObject.FindWithTag("World");
			_roads = _world.transform.Find("Roads").gameObject;
			
			LoadDifficultyConfig();
			RandomizeRoadDirections();
			SpeedUpCars();
			StartCoroutine(CarSpawnCoroutine());
		}

		private void LoadDifficultyConfig()
		{
			switch (MinigameManager.minigameInfo.difficulty)
			{
				case MinigameManager.Difficulty.Hard:
					playerMovementSpeed = hardDifficultyConfig.playerMovementSpeed;
					carMovementSpeed = hardDifficultyConfig.carMovementSpeed;
					carSpawnTime = hardDifficultyConfig.carSpawnTime;
					break;
				case MinigameManager.Difficulty.Medium:
					playerMovementSpeed = mediumDifficultyConfig.playerMovementSpeed;
					carMovementSpeed = mediumDifficultyConfig.carMovementSpeed;
					carSpawnTime = mediumDifficultyConfig.carSpawnTime;
					break;
				case MinigameManager.Difficulty.Easy:
					playerMovementSpeed = easyDifficultyConfig.playerMovementSpeed;
					carMovementSpeed = easyDifficultyConfig.carMovementSpeed;
					carSpawnTime = easyDifficultyConfig.carSpawnTime;
					break;
			}
		}

		private void RandomizeRoadDirections()
		{
			_roadSpawnVectors = new Vector2[_roads.transform.childCount];
			for (var i = 0; i < _roads.transform.childCount; i++)
			{
				var road = _roads.transform.GetChild(i).gameObject;
				if (Random.Range(0, 2) == 0)
					_roadSpawnVectors[i] = new Vector2(road.transform.position.x, 10);
				else
					_roadSpawnVectors[i] = _roadSpawnVectors[i] = new Vector2(road.transform.position.x, -10);
			}
		}

		private void SpeedUpCars()
		{
			carMovementSpeed *= carSpeedMultiplier;
			carSpawnTime *= carSpawnMultiplier;
		}

		private void SlowDownCars()
		{
			carMovementSpeed /= carSpeedMultiplier;
			carSpawnTime /= carSpawnMultiplier;
		}

		public override void OnMinigameStart()
		{
			base.OnMinigameStart();
			SlowDownCars();
		}

		private IEnumerator CarSpawnCoroutine()
		{
			while (true)
			{
				for (var i = 0; i < _roads.transform.childCount; i++)
				{
					StartCoroutine(SpawnCar(_roadSpawnVectors[i]));
				}
				yield return new WaitForSeconds(carSpawnTime);
			}
		}

		private IEnumerator SpawnCar(Vector2 position)
		{
			// Wait for a short, arbitrary duration to make the cars more random 
			yield return new WaitForSeconds(Random.Range(0f, carSpawnTime * maxSpawnFluctuationPercentage));

			var car = Instantiate(carPrefab, position, Quaternion.identity);
			car.transform.parent = _characters.transform;
			cars.Add(car);
		}

		private void Update()
		{
			carTriggers = cars.Select(x => x.transform.GetChild(0).gameObject).ToList();
		}
	}
}
