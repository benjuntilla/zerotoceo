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
        private bool _triggeredLevelUpPopup, _isReady = true;
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
        }

        public void Queue(string id)
        {
            _queue.Enqueue(id);
        }

        private void Trigger()
        {
            switch (_queue.Dequeue())
            {
                case "minigame":
                    _text.SetText("A minigame is now playable! Go to the exit door to start.");
                    PopInThenOut(5);
                    break;
                case "save":
                    _text.SetText("fsfjselfjlskfjklsejflksjeklfjlsjfsjelfjslefjsjfeklsklejfk.");
                    PopInThenOut(2);
                    break;
                case "levelUp":
                    _text.SetText("You can now level up! Go to the level door to advance.");
                    PopInThenOut(5);
                    break;
            }
        }

        private void PopInThenOut(float showDuration, float tweenSpeed = 0.75f)
        {
            _isReady = false;
            Enable();
            _cachedRectHeight = _rectTransform.rect.height;
            _cachedDuration = showDuration;
            _rectTransform.anchoredPosition = new Vector2(0, (float)(_cachedRectHeight * 1.5));
            _sequence = DOTween.Sequence();
            _sequence.Append(_rectTransform.DOAnchorPos(new Vector2(0, 0), tweenSpeed).SetSpeedBased())
                .AppendInterval(_cachedDuration)
                .Append( _rectTransform.DOAnchorPos(new Vector2(0, _cachedRectHeight), tweenSpeed).SetSpeedBased().OnComplete(() =>
                {
                    Disable();
                    _isReady = true;
                }));
            _sequence.Play();
        }

        protected override void Update()
        {
            base.Update();
            
            // Prevent sequence from using the rect height before the content size fitter updates
            if (_sequence != null && _sequence.IsActive() && _sequence.IsPlaying() && _cachedRectHeight != _rectTransform.rect.height)
                PopInThenOut(_cachedDuration);
            
            // Trigger queued popups
            if (_queue.Count > 0 && _isReady)
                Trigger();

            // Trigger the level up popup when appropriate
            if (_levelManager.currentLevelType == LevelManager.LevelType.Level && _player.points >= _levelManager.nextLevelRequirements[_levelManager.levelIndex] && !_triggeredLevelUpPopup)
            {
                _triggeredLevelUpPopup = true;
                Queue("levelUp");
            }
        }
    }
}
