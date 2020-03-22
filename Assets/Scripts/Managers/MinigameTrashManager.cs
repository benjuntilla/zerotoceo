using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MinigameManager))]
public class MinigameTrashManager : Minigame
{
    private int _trashCount;
    private GameObject _characters;
    // Config
    private float _instantiateDelay;
    private int _playerMovementSpeed;
    private bool _gameOver;

    [Header("Game Config")]
    public List<GameObject> trashPrefabs;
    public float trashTorque = 90f; 
    [HideInInspector] public float trashGravity;
    # region public config classes
    [System.Serializable]
    public class EasyDifficultyConfig
    {
        public float trashGravity = .75f;
        public int playerMovementSpeed = 8;
        public float instantiateDelay = 2f;
        public int timer = 30;
    }
    public EasyDifficultyConfig easyDifficultyConfig;
	
    [System.Serializable]
    public class MediumDifficultyConfig
    {
        public float trashGravity = .75f;
        public int playerMovementSpeed = 10;
        public float instantiateDelay = 1.5f;
        public int timer = 45;
    }
    public MediumDifficultyConfig mediumDifficultyConfig;
	
    [System.Serializable]
    public class HardDifficultyConfig
    {
        public float trashGravity = .75f;
        public int playerMovementSpeed = 12;
        public float instantiateDelay = 1f;
        public int timer = 60;
    }
    public HardDifficultyConfig hardDifficultyConfig;
    #endregion

    void Start()
    {
        _characters = GameObject.Find("Characters");
        
        LoadDifficultyConfig();
    }

    public override void OnMinigameStart()
    {
        base.OnMinigameStart();
        StartCoroutine(InstantiateTrashLoop());
    }

    private void LoadDifficultyConfig()
    {
        switch (MinigameManager.minigameInfo.difficulty)
        {
            case MinigameManager.Difficulty.Hard:
                trashGravity = hardDifficultyConfig.trashGravity;
                _playerMovementSpeed = hardDifficultyConfig.playerMovementSpeed;
                _instantiateDelay = hardDifficultyConfig.instantiateDelay;
                timerStartTime = hardDifficultyConfig.timer;
                break;
            case MinigameManager.Difficulty.Medium:
                trashGravity = mediumDifficultyConfig.trashGravity;
                _playerMovementSpeed = mediumDifficultyConfig.playerMovementSpeed;
                _instantiateDelay = mediumDifficultyConfig.instantiateDelay;
                timerStartTime = mediumDifficultyConfig.timer;
                break;
            case MinigameManager.Difficulty.Easy:
                trashGravity = easyDifficultyConfig.trashGravity;
                _playerMovementSpeed = easyDifficultyConfig.playerMovementSpeed;
                _instantiateDelay = easyDifficultyConfig.instantiateDelay;
                timerStartTime = easyDifficultyConfig.timer;
                break;
        }
        
        minigamePlayer.movementSpeed = _playerMovementSpeed;
    }

    private IEnumerator InstantiateTrashLoop()
    {
        while (true)
        {
            var trash = Instantiate(trashPrefabs[Random.Range(0, trashPrefabs.Count)], new Vector3(Random.Range(-8f, 8f), 6f, 0f), Quaternion.identity);
            trash.transform.parent = _characters.transform;
            yield return new WaitForSeconds(_instantiateDelay);
        }
    }
}
