using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BotBrain : MonoBehaviour
{

    public Transform myTarget;
    public Bot botIWantToDestroy;
    public Bot bot;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newClone = new GameObject();
        myTarget = newClone.transform;
        bot = GetComponent<Bot>();
        GetATarget(null);
        StartCoroutine(SetTarget(Random.Range(0f, 1.5f)));
    }

    // Update is called once per frame
    void Update()
    {

        if (botIWantToDestroy == null)
        {
            GetATarget(null);
        }

        Vector3 targetDirection = myTarget.transform.position - transform.position;
        targetDirection.y = 0f;
        bot.movementInput = targetDirection;
    }

    IEnumerator SetTarget(float _time)
    {
        yield return new WaitForSeconds(_time);
        myTarget.position = botIWantToDestroy.transform.position;
        StartCoroutine(SetTarget(Random.Range(0f,1.5f)));
    }

    public void GetATarget(Bot deadBot)
    {
        Bot[] possibleTargets = FindObjectsOfType<Bot>();
        botIWantToDestroy = possibleTargets[Random.Range(0, possibleTargets.Length)];
        if (botIWantToDestroy == bot || (deadBot!=null&&botIWantToDestroy==deadBot))
        {
            botIWantToDestroy = null;
            return;
        }
        botIWantToDestroy.GetComponent<Health>().OnDeath.AddListener(GetATarget);
    }

}
