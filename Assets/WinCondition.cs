using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public static WinCondition singleton;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Debug.LogWarning("Too many WinConditionObjects", gameObject);
    }

    public int botCount = 0;

    public void DeathOf(Bot bot)
    {

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
