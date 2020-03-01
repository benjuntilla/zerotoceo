using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Ink.Runtime;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
[RequireComponent(typeof(MinigameManager))]
[RequireComponent(typeof(LevelManager))]
[RequireComponent(typeof(DialogueManager))]
public class SaveManager : MonoBehaviour
{
    public static bool loadFlag { get; private set; }

    private UIManager _uiManager;
    private MinigameManager _minigameManager;
    private LevelManager _levelManager;
    private DialogueManager _dialogueManager;
    private GameObject _player;
    private GameObject[] _npcList;
    private PlayerController _playerController;

    void Awake ()
    {
        _levelManager = GetComponent<LevelManager>();
        _uiManager = FindObjectOfType<UIManager>();
        _uiManager.uiReadyEvent.AddListener(CheckLoadOrNew);
        _minigameManager = GetComponent<MinigameManager>();
        _dialogueManager = GetComponent<DialogueManager>();
        
        if (_levelManager.currentLevelType != LevelManager.LevelType.Level) return;
        _player = GameObject.FindWithTag("Player");
        _npcList = GameObject.FindGameObjectsWithTag("NPC");
        _playerController = _player.GetComponent<PlayerController>();
    }

    [System.Serializable]
    public class Data
    {
        public int level;
        public int lives;
        public int points;
        public Dictionary<string, float[]> characterPositions;
        public Dictionary<string, string> dialogueData;
        public Dictionary<string, int> scoreboard;
        public string minigame;
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
        else if (_levelManager.currentLevelType == LevelManager.LevelType.Level) // FOR DEBUG PURPOSES
        {
            New();
        }
    }

    public void Save ()
    {
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
            points = PlayerController.points,
            lives = PlayerController.lives,
            dialogueData = _dialogueManager.sessionDialogueData,
            characterPositions = characterPositions,
            minigame = MinigameManager.minigameId,
            minigameProgression = _minigameManager.minigameProgression,
            gameFlags = gameFlags
        };

        // Save data into system
        var formatter = new BinaryFormatter();
        var path = Application.persistentDataPath + "/savedata";
        var stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        
        _uiManager.QueuePopup("save");
    }

    public void Load ()
    {
        // Retrieve data from system
        var path = Application.persistentDataPath + "/savedata";
        if (!File.Exists(path)) return;
        var formatter = new BinaryFormatter();
        var stream = new FileStream(path, FileMode.Open);
        var data = formatter.Deserialize(stream) as Data;
        stream.Close();
        if (data == null) return;
        
        // Apply data
        _levelManager.levelIndex = data.level;
        _levelManager.scoreboard = data.scoreboard;
        _playerController.SetPoints(data.points);
        _playerController.SetLives(data.lives);
        _dialogueManager.sessionDialogueData = data.dialogueData;
        MinigameManager.minigameId = data.minigame;
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

    private int GetSavedLevelIndex()
    {
        // Retrieve data from system
        var path = Application.persistentDataPath + "/savedata";
        if (!File.Exists(path)) return 0;
        var formatter = new BinaryFormatter();
        var stream = new FileStream(path, FileMode.Open);
        var data = formatter.Deserialize(stream) as Data;
        stream.Close();
        return data == null ? 0 : data.level;
    }
    
    private void New ()
    {
        MinigameManager.minigameId = "";
        _playerController.SetPoints(0);
        _playerController.SetLives(3);
    }

    public void LoadSavedLevel()
    {
        loadFlag = true;
        _levelManager.LoadLevel(GetSavedLevelIndex());
    }

    public void LoadNewLevel(object param)
    {
        loadFlag = false;
        _levelManager.LoadLevel(param);
    }
}
