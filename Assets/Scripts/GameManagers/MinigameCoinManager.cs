using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.Remoting;
using System.Timers;
using Prime31;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MinigameManager))]
public class MinigameCoinManager : MonoBehaviour
{
    private GameObject _ui, _characters, _dropRegions, _pennyRegion, _nickelRegion, _dimeRegion, _quarterRegion;
    private TextMeshProUGUI _timerText;
    private MinigameManager _minigameManager;
    private IEnumerator _instantiateLoop;
    // Config
    private float _instantiateDelay;
    private int _timer;
    
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
        _ui = GameObject.FindWithTag("UI");
        _timerText = _ui.transform.Find("HUD").gameObject.transform.Find("Timer").gameObject.GetComponent<TextMeshProUGUI>();
        _minigameManager = GetComponent<MinigameManager>();
        _characters = GameObject.Find("Characters");
        _dropRegions = GameObject.Find("Minigame World").transform.Find("Drop Regions").gameObject;
        _pennyRegion = _dropRegions.transform.Find("Penny Region").gameObject;
        _nickelRegion = _dropRegions.transform.Find("Nickel Region").gameObject;
        _dimeRegion = _dropRegions.transform.Find("Dime Region").gameObject;
        _quarterRegion = _dropRegions.transform.Find("Quarter Region").gameObject;

        _instantiateLoop = InstantiateLoop();
        _timerText.SetText($"Time left: {_timer} seconds");
        LoadDifficultyConfig();
        Invoke(nameof(StartGame), 3f);
    }

    private void StartGame()
    {
        StartCoroutine(_instantiateLoop);
        StartCoroutine(Timer());
    }
    
    private void LoadDifficultyConfig()
    {
        switch (MinigameManager.MinigameDifficulty)
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
                coin.GetComponent<CollidableController>().secondaryTargetObject = _pennyRegion;
                coin.GetComponent<CollidableController>().targetObjectList.Add(_nickelRegion);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_dimeRegion);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_quarterRegion);
                break;
            case 1:
                coin = Instantiate(nickelPrefab, new Vector3(Random.Range(-8f, 8f), 4f, 0f), Quaternion.identity);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_pennyRegion);
                coin.GetComponent<CollidableController>().secondaryTargetObject = _nickelRegion;
                coin.GetComponent<CollidableController>().targetObjectList.Add(_dimeRegion);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_quarterRegion);
                break;
            case 2:
                coin = Instantiate(dimePrefab, new Vector3(Random.Range(-8f, 8f), 4f, 0f), Quaternion.identity);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_pennyRegion);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_nickelRegion);
                coin.GetComponent<CollidableController>().secondaryTargetObject = _dimeRegion;
                coin.GetComponent<CollidableController>().targetObjectList.Add(_quarterRegion);
                break;
            case 3:
                coin = Instantiate(quarterPrefab, new Vector3(Random.Range(-8f, 8f), 4f, 0f), Quaternion.identity);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_pennyRegion);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_nickelRegion);
                coin.GetComponent<CollidableController>().targetObjectList.Add(_dimeRegion);
                coin.GetComponent<CollidableController>().secondaryTargetObject = _quarterRegion;
                break;
        }
        coin.GetComponent<CollidableController>().collisionMethod.AddListener(delegate { coin.GetComponent<CollidableController>().collisionEventsEnabled = false; });
        coin.GetComponent<CollidableController>().collisionMethod.AddListener(_minigameManager.Fail);
        coin.GetComponent<CollidableController>().secondaryCollisionMethod.AddListener(delegate { Destroy(coin);});
        coin.GetComponent<DraggableController>().dragMethod.AddListener(delegate { coin.GetComponent<CollidableController>().collisionEventsEnabled = false; });
        coin.GetComponent<DraggableController>().dropMethod.AddListener(delegate { coin.GetComponent<CollidableController>().collisionEventsEnabled = true; });
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
