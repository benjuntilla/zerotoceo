using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting;
using System.Timers;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MinigameManager))]
public class MinigameCoinManager : MonoBehaviour, IMinigameManager
{
    private GameObject _characters;
    private TextMeshProUGUI _timerText;
    private MinigameManager _minigameManager;
    private IEnumerator _instantiateLoop;
    // Config
    private float _instantiateDelay;
    private int _timer;

    public bool countDownNecessary { get; set; } = true;
    [Header("Game Config")]
    public int cooldownTime = 2;
    public GameObject pennyPrefab, nickelPrefab, dimePrefab, quarterPrefab;
    # region public config classes
    [System.Serializable]
    public class EasyDifficultyConfig
    {
        public float instantiateDelay = 2f;
        public int timer = 30;
    }
    public EasyDifficultyConfig easyDifficultyConfig;
	
    [System.Serializable]
    public class MediumDifficultyConfig
    {
        public float instantiateDelay = 1.5f;
        public int timer = 45;
    }
    public MediumDifficultyConfig mediumDifficultyConfig;
	
    [System.Serializable]
    public class HardDifficultyConfig
    {
        public float instantiateDelay = 1f;
        public int timer = 60;
    }
    public HardDifficultyConfig hardDifficultyConfig;
    #endregion
    
    void Start()
    {
        _characters = GameObject.Find("Characters");
        _minigameManager = GetComponent<MinigameManager>();
        _timerText = GameObject.FindWithTag("UI").transform.Find("HUD").transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();

        _instantiateLoop = InstantiateLoop();
        _timerText.SetText($"Time left: {_timer} seconds");
        LoadDifficultyConfig();
    }

    public void StartGame()
    {
        StartCoroutine(_instantiateLoop);
        StartCoroutine(Timer());
    }
    
    private void LoadDifficultyConfig()
    {
        switch (MinigameManager.minigameDifficulty)
        {
            case "Hard":
                _instantiateDelay = hardDifficultyConfig.instantiateDelay;
                _timer = hardDifficultyConfig.timer;
                break;
            case "Medium":
                _instantiateDelay = mediumDifficultyConfig.instantiateDelay;
                _timer = mediumDifficultyConfig.timer;
                break;
            default: // "Easy"
                _instantiateDelay = easyDifficultyConfig.instantiateDelay;
                _timer = easyDifficultyConfig.timer;
                break;
        }
    }

    private void InstantiateCoin()
    {
        // Instantiate trash
        GameObject coin;
        switch (Random.Range(0, 4))
        {
            default: // case 0
                coin = Instantiate(pennyPrefab, new Vector3(Random.Range(-8f, 8f), 4f, 0f), Quaternion.identity);
                break;
            case 1:
                coin = Instantiate(nickelPrefab, new Vector3(Random.Range(-8f, 8f), 4f, 0f), Quaternion.identity);
                break;
            case 2:
                coin = Instantiate(dimePrefab, new Vector3(Random.Range(-8f, 8f), 4f, 0f), Quaternion.identity);
                break;
            case 3:
                coin = Instantiate(quarterPrefab, new Vector3(Random.Range(-8f, 8f), 4f, 0f), Quaternion.identity);
                break;
        }
        coin.transform.parent = _characters.transform;
    }

    private IEnumerator InstantiateLoop()
    {
        while (true)
        {
            InstantiateCoin();
            yield return new WaitForSeconds(_instantiateDelay);
        }
    }

    private void CheckPass()
    {
        if (_characters.transform.childCount == 0)
            _minigameManager.Pass();
        else
            _minigameManager.Fail();
    }

    private IEnumerator Timer()
    {
        for (var i = _timer; i >= 0; i--)
        {
            _timerText.SetText($"Time left: {i} seconds");
            yield return new WaitForSeconds(1);
            if (i == cooldownTime)
                StopCoroutine(_instantiateLoop);
        }
        CheckPass();
    }
}
