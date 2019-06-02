using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskScriptsToExecuteAgain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        BotConfiguator.singleton.GetAllBots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
