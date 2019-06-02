using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawn : MonoBehaviour
{

    public BotData botData;
    public bool playerBot = false;

    public void SpawnBot()
    {
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
        
    }
}
