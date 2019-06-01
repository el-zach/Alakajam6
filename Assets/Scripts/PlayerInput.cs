using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public SpinningTop top;


    // Start is called before the first frame update
    void Start()
    {
        top = GetComponent<SpinningTop>();
    }

    // Update is called once per frame
    void Update()
    {
        top.move = GetInput();
    }

    Vector3 GetInput()
    {
        Vector3 output = Camera.main.transform.forward;
        output.y = 0f;
        output = Camera.main.transform.right * Input.GetAxis("Horizontal") + output.normalized * Input.GetAxis("Vertical");
        return output;
    }

}
