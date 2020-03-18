using System.Collections;
using UnityEngine;

public abstract class Minigame : MonoBehaviour
{
    public int timerCurrentTime { get; private set; }
    [Header("Timer Config")]
    public int cooldownTime;
    public int timerStartTime;
    
    protected MinigameManager minigameManager;
    protected MinigamePlayer minigamePlayer;

    private Coroutine _timer;

    protected virtual void Awake()
    {
        minigameManager = FindObjectOfType<MinigameManager>();
        minigamePlayer = FindObjectOfType<MinigamePlayer>();
    }

    private IEnumerator TimerCoroutine()
    {
        if (timerStartTime == 0) yield break;
        for (timerCurrentTime = timerStartTime; timerCurrentTime >= 1; timerCurrentTime--)
        {
            if (timerCurrentTime <= cooldownTime)
                OnCooldownEnter();
            yield return new WaitForSeconds(1);
        }
        OnTimerDone();
    }

    public void StartTimer()
    {
        _timer = StartCoroutine(TimerCoroutine());
    }

    public void StopTimer()
    {
        if (_timer != null)
            StopCoroutine(_timer);
        timerCurrentTime = 0;
    }
    
    public virtual void OnMinigameStart()
    {
        StartTimer();
        if (minigamePlayer != null)
            minigamePlayer.blockInput = false;
    }

    protected virtual void OnCooldownEnter() { }

    protected virtual void OnTimerDone()
    {
        minigameManager.Pass();
    }
}
