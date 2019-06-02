using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Bot bot;
    public BotBrain brain;

    private void Start()
    {
        bot = GetComponent<Bot>();
        brain = GetComponent<BotBrain>();
        brain.myTarget.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //bot.movementInput = GetInput();
        UpdateBrainTarget();
    }

    void UpdateBrainTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                brain.myTarget.position = hit.point;
            }
        }
    }

    Vector3 GetInput()
    {
        Vector3 output = Camera.main.transform.forward;
        output.y = 0f;
        output = Camera.main.transform.right * Input.GetAxis("Horizontal") + output.normalized * Input.GetAxis("Vertical");
        return output;
    }

}
