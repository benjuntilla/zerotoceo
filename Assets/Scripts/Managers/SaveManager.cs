using Ink.Runtime;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UI;
using UnityEngine;

[RequireComponent(typeof(MinigameManager))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(DialogueManager))]
public class SaveManager : MonoBehaviour
{
    public static bool loadFlag;

    private MinigameManager _minigameManager;
    private LevelManager _levelManager;
    private DialogueManager _dialogueManager;
    private GameObject _player;
    private GameObject[] _npcList;
    private Player _playerController;
    private Popup _popup;
    private string _savePath;
    
    void Awake()
    {
        _savePath = Application.persistentDataPath + "/saveData";
    }

    void Start ()
    {
        _levelManager = GetComponent<LevelManager>();
        _minigameManager = GetComponent<MinigameManager>();
        _dialogueManager = GetComponent<DialogueManager>();
        _popup = FindObjectOfType<Popup>();
        _player = GameObject.FindWithTag("Player");
        _npcList = GameObject.FindGameObjectsWithTag("NPC");
        if (_player != null)
            _playerController = _player.GetComponent<Player>();
        CheckLoadOrNew();
    }

    [System.Serializable]
    public class Data
    {
        public int level, lives, points;
        public Dictionary<string, float[]> characterPositions;
        public Dictionary<string, string> dialogueData;
        public Dictionary<string, int> scoreboard;
        public string minigame, minigameStatus;
        public int minigameProgression;
        public Dictionary<string, int> gameFlags;
    }

    private void CheckLoadOrNew()
    {
        if (loadFlag)
        {
            Load();
            loadFlag = false;
        } 
        else if (_levelManager.currentLevelType == LevelManager.LevelType.Level)
        {
            New();
        }
    }

    public void Save ()
    {
        if (_levelManager.currentLevelType == LevelManager.LevelType.Minigame) return;
        // Retrieve character positions
        var characters = new List<GameObject>(_npcList) { _player };
        var characterPositions = new Dictionary<string, float[]>();
        foreach (var controller in characters)
        {
            var controllerGameObject = controller.gameObject;
            var controllerVector = controllerGameObject.transform.position;
            var characterPosition = new float[3];
            characterPosition[0] = controllerVector.x;
            characterPosition[1] = controllerVector.y;
            characterPosition[2] = controllerVector.z;
            characterPositions.Add(controllerGameObject.name, characterPosition);
        }
        
        // Convert Ink list object to a dictionary
        var gameFlags = new Dictionary<string, int>();
        foreach (var item in _dialogueManager.gameFlags)
        {
            gameFlags.Add(item.Key.ToString(), item.Value);
        }

        // Create new Data object with more retrieved data
        var data = new Data
        {
            level = _levelManager.levelIndex,
            scoreboard = _levelManager.scoreboard,
            points = _playerController.points,
            lives = _playerController.lives,
            dialogueData = _dialogueManager.sessionDialogueData,
            characterPositions = characterPositions,
            minigame = MinigameManager.minigameId,
            minigameStatus = MinigameManager.minigameStatus.ToString(),
            minigameProgression = _minigameManager.minigameProgression,
            gameFlags = gameFlags
        };

        // Save data into system
        var formatter = new BinaryFormatter();
        var path = Application.persistentDataPath + "/savedata";
        var stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        
        _popup.Queue("save");
    }

    public void Load ()
    {
        // Retrieve data from system
        if (!File.Exists(_savePath)) return;
        var formatter = new BinaryFormatter();
        var stream = new FileStream(_savePath, FileMode.Open);
        var data = formatter.Deserialize(stream) as Data;
        stream.Close();
        if (data == null) return;
        
        // Apply data
        _levelManager.levelIndex = data.level;
        _levelManager.scoreboard = data.scoreboard;
        _playerController.points = data.points;
        _playerController.lives = data.lives;
        _dialogueManager.sessionDialogueData = data.dialogueData;
        MinigameManager.minigameId = data.minigame;
        MinigameManager.minigameStatus = (MinigameManager.Status)System.Enum.Parse( typeof(MinigameManager.Status), data.minigameStatus );
        _minigameManager.minigameProgression = data.minigameProgression;

        // Convert dictionary to InkList and apply
        var gameFlags = new InkList();
        foreach (var item in data.gameFlags)
        {
            gameFlags.Add(new InkListItem(item.Key), item.Value);
        }
        _dialogueManager.gameFlags = gameFlags;

        // Apply character positions
        foreach (KeyValuePair<string, float[]> entry in data.characterPositions)
        {
            Vector3 characterPosition;
            characterPosition.x = entry.Value[0];
            characterPosition.y = entry.Value[1];
            characterPosition.z = entry.Value[2];
            GameObject.Find(entry.Key).transform.position = characterPosition;
        }
    }

    public int GetSavedLevelIndex()
    {
        // Retrieve data from system
        if (!File.Exists(_savePath)) return 0;
        var formatter = new BinaryFormatter();
        var stream = new FileStream(_savePath, FileMode.Open);
        var data = formatter.Deserialize(stream) as Data;
        stream.Close();
        return data?.level ?? 0;
    }
    
    private void New ()
    {
        MinigameManager.minigameId = "";
        MinigameManager.minigameStatus = MinigameManager.Status.None;
        _playerController.points = 0;
        _playerController.lives = 3;
    }
}
