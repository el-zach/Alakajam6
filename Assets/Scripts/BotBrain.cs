using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BotBrain : MonoBehaviour
{

    public Transform myTarget;
    public Bot botIWantToDestroy;
    public Bot bot;
    public bool useBrain = true;

    // Start is called before the first frame update
    void Start()
    {
        GameObject newClone = new GameObject();
        myTarget = newClone.transform;
        bot = GetComponent<Bot>();
        if (useBrain)
        {
            GetATarget(null);
            StartCoroutine(SetTarget(Random.Range(0f, 1.5f)));
        }
        else
        {
            Instantiate(BotConfiguator.singleton.playerSelectionRing, myTarget);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (useBrain && botIWantToDestroy == null)
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
        if(botIWantToDestroy && botIWantToDestroy.gameObject.activeSelf)
            myTarget.position = botIWantToDestroy.transform.position;
        if(useBrain)
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
