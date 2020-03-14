using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : UIObject
    {
        private LevelManager _levelManager;
        private Player _player;
        private TextMeshProUGUI _pointsText, _timerText;
        private Minigame _minigameManagerInterface;

        public GameObject heartOne, heartTwo, heartThree, futureToken, businessToken, leaderToken, americaToken, points, tokens, hearts, timer;
        
        void Start()
        {
            _minigameManagerInterface = FindObjectsOfType<MonoBehaviour>().OfType<Minigame>().First();
            _pointsText = points.GetComponent<TextMeshProUGUI>();
            _timerText = timer.GetComponent<TextMeshProUGUI>();
            _levelManager = FindObjectOfType<LevelManager>();
            _player = FindObjectOfType<Player>();
            blocksRaycasts = false;
        }

        protected override void Update()
        {
            base.Update();
            if (!_levelManager) return;
            if (_levelManager.currentLevelType == LevelManager.LevelType.Level)
            {
                Helper.DisableChildren(gameObject);
                points.SetActive(true);
                tokens.SetActive(true);
                hearts.SetActive(true);
                Enable();

                _pointsText.SetText($"Points: {_player.points}");
        
                Helper.DisableChildren(hearts);
                switch (_player.lives)
                {
                    case 1:
                        heartOne.SetActive(true);
                        break;
                    case 2:
                        heartOne.SetActive(true);
                        heartTwo.SetActive(true);
                        break;
                    case 3:
                        heartOne.SetActive(true);
                        heartTwo.SetActive(true);
                        heartThree.SetActive(true);
                        break;
                }
        
                Helper.DisableChildren(tokens);
                switch (_levelManager.levelIndex)
                {
                    case 2:
                        futureToken.SetActive(true);
                        break;
                    case 3:
                        futureToken.SetActive(true);
                        businessToken.SetActive(true);
                        break;
                    case 4:
                        futureToken.SetActive(true);
                        businessToken.SetActive(true);
                        leaderToken.SetActive(true);
                        break;
                    case 5:
                        futureToken.SetActive(true);
                        businessToken.SetActive(true);
                        leaderToken.SetActive(true);
                        americaToken.SetActive(true);
                        break;
                }
            }
            else if (_levelManager.currentLevelType == LevelManager.LevelType.Minigame && _minigameManagerInterface.timerStartTime != 0)
            {
                Helper.DisableChildren(gameObject);
                timer.SetActive(true);
                Enable();
                _timerText.SetText($"Time left: {_minigameManagerInterface.timerCurrentTime} seconds");
            }
            else
            {
                Disable();
            }
        }
    }
}
