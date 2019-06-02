using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerCam : MonoBehaviour
{

    public List<Transform> keepInFrame;

    // Start is called before the first frame update
    void Start()
    {
        GetAllTargets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetAllTargets()
    {
        keepInFrame = new List<Transform>();
        Bot[] allBots = FindObjectsOfType<Bot>();
        foreach(var bot in allBots)
        {
            keepInFrame.Add(bot.transform);
            bot.GetComponent<Health>().OnDeath.AddListener(TakeBotOutOfTargets);
        }
    }

    public void TakeBotOutOfTargets(Bot bot)
    {
        keepInFrame.Remove(bot.transform);
    }

}
