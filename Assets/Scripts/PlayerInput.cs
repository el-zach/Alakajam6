using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Bot bot;

    private void Start()
    {
        bot = GetComponent<Bot>();
    }

    // Update is called once per frame
    void Update()
    {
        bot.movementInput = GetInput();
    }

    Vector3 GetInput()
    {
        Vector3 output = Camera.main.transform.forward;
        output.y = 0f;
        output = Camera.main.transform.right * Input.GetAxis("Horizontal") + output.normalized * Input.GetAxis("Vertical");
        return output;
    }

}
