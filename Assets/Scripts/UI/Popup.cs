using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Popup : UIObject
    {
        private TextMeshProUGUI _text;
        private Queue<string> _queue = new Queue<string>();
        private bool _triggeredLevelUpPopup;
        private LevelManager _levelManager;
        private Player _player;
        private RectTransform _rectTransform;
        private Sequence _sequence;
        private float _cachedRectHeight, _cachedDuration;

        void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _levelManager = FindObjectOfType<LevelManager>();
            _player = FindObjectOfType<Player>();
            _rectTransform = GetComponent<RectTransform>();

            _sequence = DOTween.Sequence();
        }

        public void Queue(string id)
        {
            _queue.Enqueue(id);
        }

        private void PlayTween(float showDuration, float tweenSpeed = 0.75f)
        {
            if (!_rectTransform) return;
            Enable();
            _cachedRectHeight = _rectTransform.rect.height;
            _cachedDuration = showDuration;
            _rectTransform.anchoredPosition = new Vector2(0, (float)(_cachedRectHeight * 1.5));
            _sequence = DOTween.Sequence();
            _sequence.Append(_rectTransform.DOAnchorPos(new Vector2(0, 0), tweenSpeed).SetSpeedBased())
                .AppendInterval(_cachedDuration)
                .Append(_rectTransform.DOAnchorPos(new Vector2(0, (float)(_cachedRectHeight * 1.5)), tweenSpeed).SetSpeedBased().OnComplete(Disable));
            _sequence.Play();
        }

        private void TriggerPopup()
        {
            switch (_queue.Dequeue())
            {
                case "minigame":
                    _text.SetText("A minigame is now playable! Go to the exit door to start.");
                    PlayTween(5);
                    break;
                case "save":
                    _text.SetText("Game Saved.");
                    PlayTween(2);
                    break;
                case "levelUp":
                    _text.SetText("You can now level up! Go to the level door to advance.");
                    PlayTween(5);
                    break;
            }
        }

        private void RestartPopup()
        {
            _sequence.Kill();
            PlayTween(_cachedDuration);
        }

        protected override void Update()
        {
            base.Update();
            
            // Prevent sequence from using the rect height before the content size fitter updates
            if (_cachedRectHeight != _rectTransform.rect.height && Helper.IsTweenPlaying(_sequence))
                RestartPopup();
            
            // Trigger queued popups
            if (_queue.Count > 0 && !Helper.IsTweenPlaying(_sequence))
                TriggerPopup();

            // Trigger the level up popup when appropriate
            if (_levelManager.currentLevelType == LevelManager.LevelType.Level && _player.points >= _levelManager.nextLevelRequirements[_levelManager.levelIndex] && !_triggeredLevelUpPopup)
            {
                _triggeredLevelUpPopup = true;
                Queue("levelUp");
            }
        }
    }
}
