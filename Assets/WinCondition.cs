using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public static WinCondition singleton;
    private void Awake()
    {
        if (!singleton)
            singleton = this;
        else
        {
            singleton = this;
            Debug.LogWarning("Too many WinConditionObjects", gameObject);
        }
    }

    public UnityEngine.Events.UnityEvent OnWin, OnLose;

    public int botCount = 0;
    public Bot playerBot;

    public bool roundIsDecided = false;

    public void DeathOf(Bot bot)
    {
        botCount--;
        if(botCount==1 && bot != playerBot && playerBot.gameObject.activeSelf && !roundIsDecided)
        {
            roundIsDecided = true;
            OnWin.Invoke();
        }
    }

    public void PlayerBotWasSpawned(Bot _bot)
    {
        playerBot = _bot;
        _bot.GetComponent<Health>().OnDeath.AddListener(PlayerDeath);
    }

    void PlayerDeath(Bot _bot)
    {
        if (!roundIsDecided)
        {
            roundIsDecided = true;
            OnLose.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
