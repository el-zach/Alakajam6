using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyOnLoadBehaviour : MonoBehaviour
{
    public static DontDestroyOnLoadBehaviour singleton;
    public bool skippingAvailable = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (singleton == null)
            singleton = this;
        else
            Debug.LogWarning("Too many DontDestroyOnLoadBehaviours", gameObject);
    }

    public void LoadSceneAt(int i)
    {
        SceneManager.LoadScene(i);
    }

    private void Update()
    {
        if (skippingAvailable && Input.GetKeyDown(KeyCode.N))
        {
            BotConfiguator.singleton.NameBot();
            BotConfiguator.singleton.playerBotData = BotConfiguator.singleton.myBotData;
            LoadSceneAt(3);
        }
    }

}
