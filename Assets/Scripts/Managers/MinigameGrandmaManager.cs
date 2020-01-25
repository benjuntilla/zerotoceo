using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MinigameManager))]
public class MinigameGrandmaManager : MonoBehaviour, IMinigameManager
{
	private MinigameManager _minigameManager;
	private GameObject _player, _world, _roads, _characters;
	private CollidableController _playerCollidableController;
	private IMinigamePlayer _playerScript;
	private int _roadCount;
	private int[] _roadPopulations;
	private string[] _roadDirections;
	private float[] _roadWaitingTimes;
	private int _maxRoadPopulation;
	private float _carDecayTime, _carWaitTime, _carMovementSpeed, _playerMovementSpeed;

	public bool countDownNecessary { get; set; } = false;
	[Header("Game Config")]
	public GameObject carPrefab;
	public List<Sprite> carSprites;
	#region public config classes
	[System.Serializable]
	public class EasyDifficultyConfig
	{
		public int maxRoadPopulation = 8;
		public float playerMovementSpeed = 4;
		public float carMovementSpeed = 4;
		public float carDecayTime = 3f;
		public float carWaitTime = 1f;
	}
	public EasyDifficultyConfig easyDifficultyConfig;
	
	[System.Serializable]
	public class MediumDifficultyConfig
	{
		public int maxRoadPopulation = 8;
		public float playerMovementSpeed = 4;
		public float carMovementSpeed = 4;
		public float carDecayTime = 3f;
		public float carWaitTime = 1f;
	}
	public MediumDifficultyConfig mediumDifficultyConfig;
	
	[System.Serializable]
	public class HardDifficultyConfig
	{
		public int maxRoadPopulation = 8;
		public float playerMovementSpeed = 4;
		public float carMovementSpeed = 4;
		public float carDecayTime = 3f;
		public float carWaitTime = 1f;
	}
	public HardDifficultyConfig hardDifficultyConfig;
	#endregion
	
	void Awake()
	{
		_player = GameObject.FindWithTag("Player").gameObject;
		_playerScript = _player.GetComponent<IMinigamePlayer>();
		_playerCollidableController = _player.GetComponent<CollidableController>();
		_minigameManager = GetComponent<MinigameManager>();
		_characters = GameObject.Find("Characters");
		_world = GameObject.FindWithTag("World");
		_roads = _world.transform.Find("Roads").gameObject;
		_roadCount = _roads.transform.childCount;
		_roadPopulations = new int[_roadCount];
		_roadDirections = new string[_roadCount];
		_roadWaitingTimes = new float[_roadCount];
		
		// Randomize road directions
		for (var i = 0; i < _roadDirections.Length; i++)
		{
			if (Random.Range(0, 2) == 0)
				_roadDirections[i] = "up";
			else
				_roadDirections[i] = "down";
		}
		
		LoadDifficultyConfig();
	}

	public void StartGame()
	{
	}

	private void LoadDifficultyConfig()
	{
		switch (MinigameManager.minigameDifficulty)
		{
			case "Hard":
				_maxRoadPopulation = hardDifficultyConfig.maxRoadPopulation;
				_playerMovementSpeed = hardDifficultyConfig.playerMovementSpeed;
				_carMovementSpeed = hardDifficultyConfig.carMovementSpeed;
				_carDecayTime = hardDifficultyConfig.carDecayTime;
				_carWaitTime = hardDifficultyConfig.carWaitTime;
				break;
			case "Medium":
				_maxRoadPopulation = mediumDifficultyConfig.maxRoadPopulation;
				_playerMovementSpeed = mediumDifficultyConfig.playerMovementSpeed;
				_carMovementSpeed = mediumDifficultyConfig.carMovementSpeed;
				_carDecayTime = mediumDifficultyConfig.carDecayTime;
				_carWaitTime = mediumDifficultyConfig.carWaitTime;
				break;
			default: // "Easy"
				_maxRoadPopulation = easyDifficultyConfig.maxRoadPopulation;
				_playerMovementSpeed = easyDifficultyConfig.playerMovementSpeed;
				_carMovementSpeed = easyDifficultyConfig.carMovementSpeed;
				_carDecayTime = easyDifficultyConfig.carDecayTime;
				_carWaitTime = easyDifficultyConfig.carWaitTime;
				break;
		}
		
		_playerScript.movementSpeed = _playerMovementSpeed;
	}

	private IEnumerator InstantiateCar()
	{
		// Randomize road
		var road = Random.Range(0, _roadCount);
		while (_roadPopulations[road] >= _maxRoadPopulation) // Keep randomizing roads until the random road's population is less than the maximum count
			road = Random.Range(0, _roadCount);
		_roadPopulations[road]++;

		// Determine spawn positions
		var roadPosX = _roads.transform.GetChild(road).position.x;
		var roadPosY = 0f;
		switch (_roadDirections[road])
		{
			case "up":
				roadPosY = -10f;
				break;
			case "down":
				roadPosY = 10f;
				break;
		}

		// Wait between cars to create a gap
		if (_roadPopulations[road] - 1 > 0) // Wait if the road population (not including this car) is greater than 0
		{
			_roadWaitingTimes[road] += _carWaitTime;
			var thisWaitingTime = _roadWaitingTimes[road];
			yield return new WaitForSeconds(thisWaitingTime);
			_roadWaitingTimes[road] -= _carWaitTime;
		}
		
		// Instantiate car
		var car = Instantiate(carPrefab, new Vector3(roadPosX, roadPosY, 0f), Quaternion.identity);
		car.transform.parent = _characters.transform;
		
		// Add to the player's target collisions
		_playerCollidableController.secondaryCollisionObjects.Add(car);

		// Set random sprite
		car.GetComponent<SpriteRenderer>().sprite = carSprites[Random.Range(0, carSprites.Count)];
		
		// Move car & flip according to direction
		var time = 0f;
		switch (_roadDirections[road])
		{
			case "up":
				car.transform.localScale = new Vector3(car.transform.localScale.x, -car.transform.localScale.y, car.transform.localScale.z);
				while (time < _carDecayTime)
				{
					time += Time.deltaTime;
					car.transform.Translate(Vector3.up * Time.deltaTime * _carMovementSpeed);
					yield return null;
				}
				_roadPopulations[road] -= 1;
				Destroy(car);
				break;
			case "down":
				while (time < _carDecayTime)
				{
					time += Time.deltaTime;
					car.transform.Translate(Vector3.down * Time.deltaTime * _carMovementSpeed);
					yield return null;
				}
				_roadPopulations[road] -= 1;
				Destroy(car);
				break;
		}
	}

	void Update()
	{
		if (_roadPopulations.Sum() < _roadCount * _maxRoadPopulation)
			StartCoroutine(InstantiateCar());
	}
}

