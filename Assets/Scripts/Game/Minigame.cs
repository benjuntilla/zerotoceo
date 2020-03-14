using System.Collections;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public int timerCurrentTime { get; private set; }
    public int cooldownTime;
    public int timerStartTime;
    
    protected MinigameManager minigameManager;

    protected virtual void Awake()
    {
        minigameManager = FindObjectOfType<MinigameManager>();
    }

    public virtual void StartMinigame()
    {
        StartCoroutine(StartTimer());
    }

    protected IEnumerator StartTimer()
    {
        if (timerStartTime != 0);
        {
            for (timerCurrentTime = timerStartTime; timerCurrentTime >= 1; timerCurrentTime--)
            {
                if (timerCurrentTime <= cooldownTime)
                    OnCooldownEnter();
                yield return new WaitForSeconds(1);
            }
            OnTimerDone();
        }
    }

    protected virtual void OnCooldownEnter() { }

    protected virtual void OnTimerDone()
    {
        minigameManager.Pass();
    }
}
