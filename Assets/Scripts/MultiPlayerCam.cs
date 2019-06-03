using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerCam : MonoBehaviour
{

    public List<Transform> keepInFrame;
    public Vector3 offset;
    public bool isReady = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDoStart(0.5f));
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (keepInFrame.Count == 0 && isReady)
        {
            GetAllTargets();
        }

        if(isReady)
            transform.position = GetCenterPoint() + offset;
    }

    IEnumerator WaitToDoStart(float _time)
    {
        yield return new WaitForSeconds(_time);
        GetAllTargets();
        isReady = true;
    }

    public Vector3 GetCenterPoint()
    {
        if (keepInFrame.Count == 0)
            return Vector3.zero;
        float allX=0f, allZ=0f;
        foreach(var trans in keepInFrame)
        {
            allX += trans.position.x;
            allZ += trans.position.z;
        }
        allX /= keepInFrame.Count;
        allZ /= keepInFrame.Count;
        return new Vector3(allX, 0f, allZ);
    }

    public void GetAllTargets()
    {
        keepInFrame = new List<Transform>();
        Bot[] allBots = FindObjectsOfType<Bot>();
        Debug.Log("[Cam] Found " + allBots.Length + " targets");
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
