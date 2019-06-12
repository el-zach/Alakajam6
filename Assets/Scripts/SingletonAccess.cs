using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonAccess : MonoBehaviour
{

    public void GenerateRandomBot()
    {
        BotConfiguator.singleton.GenerateRandomBot();
    }

    public void RandomColor()
    {
        BotConfiguator.singleton.ColorizeRandom();
    }

    public void NameBot()
    {
        BotConfiguator.singleton.NameBot();
    }
    public void SendActiveBot()
    {
        BotConfiguator.singleton.SendActiveBot();
    }
    public void LoadSceneAt(int sceneIndex)
    {
        DontDestroyOnLoadBehaviour.singleton.LoadSceneAt(sceneIndex);
    }
}
