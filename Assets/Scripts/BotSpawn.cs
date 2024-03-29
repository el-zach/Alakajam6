﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawn : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEngine.Events.UnityEvent<Bot> { }

    public BotData botData;
    public bool playerBot = false;

    public BotSpawn.Event OnSpawn;

    public void SpawnBot()
    {
        try { 
            if (botData == null && BotConfiguator.singleton.possibleBots.Count>0)
            {
                botData = BotConfiguator.singleton.possibleBots[Random.Range(0, BotConfiguator.singleton.possibleBots.Count)];
            }
            if (botData)
            {
                var newBot = BotConfiguator.GenerateBotFromData(botData, playerBot, false, transform);
                var healthScript = newBot.GetComponent<Health>();
                var botScript = newBot.GetComponent<Bot>();
                if (WinCondition.singleton)
                {
                    WinCondition.singleton.botCount++;
                    healthScript.OnDeath.AddListener(WinCondition.singleton.DeathOf);
                    if (playerBot)
                        WinCondition.singleton.playerBot = botScript;
                }
                var camScript = Camera.main.GetComponent<MultiPlayerCam>();
                if (camScript)
                {
                    camScript.keepInFrame.Add(newBot.transform);
                    healthScript.OnDeath.AddListener(camScript.TakeBotOutOfTargets);
                }
                OnSpawn.Invoke(botScript);
            }
        } 
        catch(System.Exception e)
        {
            Debug.LogWarning("exception on bot creation: " + e);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnBot();
    }

    // Update is called once per frame
    void Update()
    {
        if (!botData)
            SpawnBot();
    }

    private void OnDrawGizmos()
    {
        if (!playerBot)
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 1f);
    }
}
