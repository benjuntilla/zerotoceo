using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Runtime.Remoting;
using System.Timers;
using UnityEngine;

[RequireComponent(typeof(MinigameManager))]
public class MinigameTrashManager : MonoBehaviour, IMinigameManager
{
    private int _trashCount;
    private GameObject _characters, _ui;
    private TextMeshProUGUI _timerText;
    private MinigameManager _minigameManager;
    // Config
    private float _instantiateDelay;
    private int _timer, _playerMovementSpeed;
    private bool _gameOver;
    
    public bool countDownNecessary { get; set; } = true;
    [Header("Game Config")]
    public List<GameObject> trashPrefabs;
    public float trashTorque = 90f, trashGravity;
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
        _ui = GameObject.FindWithTag("UI");
        _timerText = _ui.transform.Find("HUD").gameObject.transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();
        _minigameManager = GetComponent<MinigameManager>();
        _characters = GameObject.Find("Characters");
        
        _timerText.SetText("Time left: 0 seconds");
        LoadDifficultyConfig();
    }

    public void StartGame()
    {
        StartCoroutine(InstantiateLoop());
        StartCoroutine(Timer());
    }
    
    private void LoadDifficultyConfig()
    {
        switch (MinigameManager.minigameDifficulty)
        {
            case "Hard":
                trashGravity = hardDifficultyConfig.trashGravity;
                _playerMovementSpeed = hardDifficultyConfig.playerMovementSpeed;
                _instantiateDelay = hardDifficultyConfig.instantiateDelay;
                _timer = hardDifficultyConfig.timer;
                break;
            case "Medium":
                trashGravity = mediumDifficultyConfig.trashGravity;
                _playerMovementSpeed = mediumDifficultyConfig.playerMovementSpeed;
                _instantiateDelay = mediumDifficultyConfig.instantiateDelay;
                _timer = mediumDifficultyConfig.timer;
                break;
            default: // "Easy"
                trashGravity = easyDifficultyConfig.trashGravity;
                _playerMovementSpeed = easyDifficultyConfig.playerMovementSpeed;
                _instantiateDelay = easyDifficultyConfig.instantiateDelay;
                _timer = easyDifficultyConfig.timer;
                break;
        }
        
        GameObject.FindWithTag("Player").GetComponent<IMinigamePlayer>().movementSpeed = _playerMovementSpeed;
    }

    private void InstantiateTrash()
    {
        var trash = Instantiate(trashPrefabs[Random.Range(0, trashPrefabs.Count)], new Vector3(Random.Range(-8f, 8f), 6f, 0f), Quaternion.identity);
        trash.transform.parent = _characters.transform;
    }

    private IEnumerator InstantiateLoop()
    {
        while (true)
        {
            InstantiateTrash();
            yield return new WaitForSeconds(_instantiateDelay);
        }
    }

    private IEnumerator Timer()
    {
        for (var i = _timer; i >= 0; i--)
        {
            _timerText.SetText($"Time left: {i} seconds");
            yield return new WaitForSeconds(1);
        }
        _minigameManager.Pass();
    }

    public void TestFail()
    {
        if (MinigameManager.minigameStatus == MinigameManager.Status.InProgress)
            _minigameManager.Fail();
    }
}
