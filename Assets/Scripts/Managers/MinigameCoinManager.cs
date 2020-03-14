using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(MinigameManager))]
public class MinigameCoinManager : Minigame
{
    private GameObject _characters;
    private Coroutine _instantiateLoop;
    // Config
    private float _instantiateDelay;

    [Header("Game Config")]
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

        LoadDifficultyConfig();
    }

    public override void StartMinigame()
    {
        _instantiateLoop = StartCoroutine(InstantiateLoop());
        StartCoroutine(StartTimer());    
    }

    private void LoadDifficultyConfig()
    {
        switch (MinigameManager.minigameDifficulty)
        {
            case "Hard":
                _instantiateDelay = hardDifficultyConfig.instantiateDelay;
                timerStartTime = hardDifficultyConfig.timer;
                break;
            case "Medium":
                _instantiateDelay = mediumDifficultyConfig.instantiateDelay;
                timerStartTime = mediumDifficultyConfig.timer;
                break;
            default: // case "Easy":
                _instantiateDelay = easyDifficultyConfig.instantiateDelay;
                timerStartTime = easyDifficultyConfig.timer;
                break;
        }
    }

    private void InstantiateCoin()
    {
        GameObject coin;
        switch (Random.Range(0, 4))
        {
            default: // case 0:
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
    
    protected override void OnTimerDone()
    {
        if (_characters.transform.childCount == 0)
            minigameManager.Pass();
        else
            minigameManager.Fail();    
    }

    protected override void OnCooldownEnter()
    {
        StopCoroutine(_instantiateLoop);
    }
}
