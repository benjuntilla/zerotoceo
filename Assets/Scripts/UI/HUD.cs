using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUD : UIObject
    {
        private LevelManager _levelManager;
        private Player _player;
        private TextMeshProUGUI _pointsNumberText, _timerText, _countdownText, _pointsIndicatorText;
        private Minigame _minigame;
        private Slider _timerSlider, _countdownSlider;
        private bool _countdownTriggered, _countdownInProgress;
        private CanvasGroup _countdownCanvasGroup, _pointsIndicatorCanvasGroup;
        private int _cachedPoints;
        private Sequence _pointsIndicatorSequence;

        public GameObject heartOne, heartTwo, heartThree, futureToken, businessToken, leaderToken, americaToken, points, tokens, hearts, timer, countdown, pointsIndicator, pointsNumber;

        void Start()
        {
            _minigame = FindObjectOfType<Minigame>();
            _pointsNumberText = pointsNumber.GetComponent<TextMeshProUGUI>();
            _countdownText = countdown.GetComponentInChildren<TextMeshProUGUI>();
            _countdownSlider = countdown.GetComponentInChildren<Slider>();
            _timerText = timer.GetComponentInChildren<TextMeshProUGUI>();
            _timerSlider = timer.GetComponentInChildren<Slider>();
            _countdownCanvasGroup = countdown.GetComponentInChildren<CanvasGroup>();
            _pointsIndicatorCanvasGroup = pointsIndicator.GetComponent<CanvasGroup>();
            _pointsIndicatorText = pointsIndicator.GetComponent<TextMeshProUGUI>();
            
            _levelManager = FindObjectOfType<LevelManager>();
            _player = FindObjectOfType<Player>();
            blocksRaycasts = false;
        }

        private void UpdateLevelHUD()
        {
            Helper.DisableChildren(gameObject);
            points.SetActive(true);
            tokens.SetActive(true);
            hearts.SetActive(true);
            Enable();

            _pointsNumberText.SetText(_player.points.ToString());
            if (_cachedPoints != _player.points)
            {
                _pointsIndicatorText.SetText($"+{_player.points - _cachedPoints}");
                _cachedPoints = _player.points;
                TriggerPointsIndicator();
            }
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

        public void TriggerPointsIndicator()
        {
            if (_pointsIndicatorSequence != null && _pointsIndicatorSequence.IsActive() && _pointsIndicatorSequence.IsPlaying()) return;
            _pointsIndicatorSequence = DOTween.Sequence();
            _pointsIndicatorSequence.Append(_pointsIndicatorCanvasGroup.DOFade(0, 1.5f)).Join(            
                pointsIndicator.gameObject.transform.DOMoveY(pointsIndicator.gameObject.transform.position.y - 10, 1.5f).OnComplete(() =>
                {
                    pointsIndicator.SetActive(false);
                    pointsIndicator.transform.Translate(new Vector3(0, 10, 0));
                }));
            pointsIndicator.SetActive(true);
            _pointsIndicatorCanvasGroup.alpha = 1;
            _pointsIndicatorSequence.Play();
        }

        private void UpdateMinigameHUD()
        {
            Helper.DisableChildren(gameObject);
            countdown.SetActive(_countdownInProgress);
            timer.SetActive(true);
            Enable();
            
            _timerText.SetText($"Time left: {_minigame.timerCurrentTime} seconds");
            _timerSlider.maxValue = _minigame.timerStartTime;
            _timerSlider.value = _minigame.timerCurrentTime;
            if (!_countdownTriggered && _minigame.timerStartTime != 0)
                StartCoroutine(TriggerCountdown());
        }

        private IEnumerator TriggerCountdown()
        {
            _countdownTriggered = _countdownInProgress = true;
            for (var i = 3; i >= 1; i--)
            {
                _countdownText.SetText(i.ToString());
                _countdownSlider.value = i;
                yield return new WaitForSeconds(1);
            }
            _countdownText.SetText("0");
            _countdownSlider.value = 0;
            _countdownCanvasGroup.DOFade(0, 0.5f).OnComplete(() => { _countdownInProgress = false; });
        }

        protected override void Update()
        {
            base.Update();
            if (!_levelManager) return;
            if (_levelManager.currentLevelType == LevelManager.LevelType.Level)
                UpdateLevelHUD();
            else if (_levelManager.currentLevelType == LevelManager.LevelType.Minigame && _minigame.timerStartTime != 0)
                UpdateMinigameHUD();
            else
                Disable();
        }
    }
}
