using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BotBrain : MonoBehaviour
{

    public Transform myTarget;
    public Bot bot;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newClone = new GameObject();
        myTarget = newClone.transform;
        bot = GetComponent<Bot>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = myTarget.transform.position - transform.position;
        targetDirection.y = 0f;
        bot.movementInput = targetDirection;
    }
}
