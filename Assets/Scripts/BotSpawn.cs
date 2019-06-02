using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawn : MonoBehaviour
{

    public BotData botData;
    public bool playerBot = false;
    public GameObject enableOnDeath;

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
                if(enableOnDeath)
                    newBot.GetComponent<Health>().OnDeath.AddListener(EnableOnDeath);
            }
        } 
        catch(System.Exception e)
        {
            Debug.LogWarning("exception on bot creation: " + e);
        }
    }

    void EnableOnDeath(Bot deadbot)
    {
        enableOnDeath.SetActive(true);
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
}
