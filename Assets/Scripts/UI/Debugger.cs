using UnityEngine;

namespace UI
{
    public class Debugger : MonoBehaviour
    {
        private bool _enabled;
        private LevelManager _levelManager;
        private Player _player;
        private Rigidbody2D _playerRb;
        private Vector2 _playerVelocity;

        public GUIStyle debugStyle;

        void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            if (_levelManager.currentLevelType == LevelManager.LevelType.Level)
            {
                _player = FindObjectOfType<Player>();
                _playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
            }
        }
        
        void OnGUI()
        {
            if (!_enabled ) return;
            var log = "";
            switch (_levelManager.currentLevelType)
            {
                case LevelManager.LevelType.Level:
                    _playerVelocity = _playerRb.velocity;
                    log =
                        $"X Velocity: {_playerVelocity.x}\n" +
                        $"Y Velocity: {_playerVelocity.y}\n" +
                        $"Points: {_player.points}\n" +
                        $"Lives: {_player.lives}\n" +
                        $"Level: {_levelManager.levelIndex}\n" +
                        $"Timescale: {Time.timeScale}\n";
                    GUI.Label(new Rect(10, 100, 999, 999), log, debugStyle); // Rectangle dimensions are as follows: (distance from left edge, distance from top edge, width, height)
                    break;
                case LevelManager.LevelType.Minigame:
                    log =
                        $"Minigame ID: {MinigameManager.minigameId}\n" +
                        $"Minigame Status: {MinigameManager.minigameStatus}\n" +
                        $"Timescale: {Time.timeScale}\n";           
                    GUI.Label(new Rect(10, 10, 999, 999), log, debugStyle); // Rectangle dimensions are as follows: (distance from left edge, distance from top edge, width, height)
                    break;
            }
        }
    
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F3))
            {
                _enabled = !_enabled;
            }
        }
    }
}
