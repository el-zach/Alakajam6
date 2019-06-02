using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawn : MonoBehaviour
{

    public BotData botData;
    public bool playerBot = false;

    public void SpawnBot()
    {

        if (botData == null && BotConfiguator.singleton.possibleBots.Count>0)
        {
            botData = BotConfiguator.singleton.possibleBots[Random.Range(0, BotConfiguator.singleton.possibleBots.Count)];
        }
        if(botData)
            BotConfiguator.GenerateBotFromData(botData, playerBot, transform);
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
