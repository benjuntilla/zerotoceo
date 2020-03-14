using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Popup : UIObject
    {
        public TextMeshProUGUI text;
        public Animator animator;
        
        private Queue<string> _queue = new Queue<string>();
        private bool _triggeredLevelUpPopup, _isReady = true;
        private LevelManager _levelManager;
        private PlayerController _playerController;

        void Start()
        {
            _levelManager = FindObjectOfType<LevelManager>();
            _playerController = FindObjectOfType<PlayerController>();
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
                    text.SetText("A minigame is now playable! Go to the exit door to start.");
                    StartCoroutine(PlayInThenOut(5));
                    break;
                case "save":
                    text.SetText("Saved game.");
                    StartCoroutine(PlayInThenOut(2));
                    break;
                case "levelUp":
                    text.SetText("You can now level up! Go to the level door to advance.");
                    StartCoroutine(PlayInThenOut(5));
                    break;
            }
        }

        private IEnumerator PlayInThenOut(int seconds)
        {
            _isReady = false;
            Enable();
            Helper.PlayIn(animator);
            yield return new WaitForSeconds(seconds);
            StartCoroutine(Helper.PlayOutThenAction(animator, () =>
            {
                Disable();
                _isReady = true;
            }));
        }

        void Update()
        {
            // Trigger queued popups
            if (_queue.Count > 0 && _isReady)
                Trigger();

            // Trigger the level up popup when appropriate
            if (_levelManager.currentLevelType == LevelManager.LevelType.Level && _playerController.points >= _levelManager.nextLevelRequirements[_levelManager.levelIndex] && !_triggeredLevelUpPopup)
            {
                _triggeredLevelUpPopup = true;
                Queue("levelUp");
            }
        }
    }
}
