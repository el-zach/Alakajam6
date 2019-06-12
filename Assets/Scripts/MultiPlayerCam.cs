using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPlayerCam : MonoBehaviour
{

    public List<Transform> keepInFrame;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = GetCenterPoint() + offset;
    }


    public Vector3 GetCenterPoint()
    {
        if (keepInFrame.Count == 0)
            return Vector3.zero;
        float allX=0f, allZ=0f;
        int number = 0;
        foreach(var trans in keepInFrame)
        {
            if (trans && trans.gameObject.activeSelf)
            {
                allX += trans.position.x;
                allZ += trans.position.z;
                number++;
            }
        }
        if (number == 0)
        {
            return Vector3.zero;
        }
        allX /= number;
        allZ /= number;
        return new Vector3(allX, 0f, allZ);
    }

    /*public void GetAllTargets()
    {
        keepInFrame = new List<Transform>();
        Bot[] allBots = FindObjectsOfType<Bot>();
        Debug.Log("[Cam] Found " + allBots.Length + " targets");
        foreach(var bot in allBots)
        {
            keepInFrame.Add(bot.transform);
            bot.GetComponent<Health>().OnDeath.AddListener(TakeBotOutOfTargets);
        }
    }*/

    public void TakeBotOutOfTargets(Bot bot)
    {
        keepInFrame.Remove(bot.transform);
    }

}
