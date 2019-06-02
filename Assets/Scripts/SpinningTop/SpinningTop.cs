using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningTop : MonoBehaviour
{
    public Rigidbody rigid;
    public Rotator rotator;

    public float speed = 1f;
    public float additionalTorque = 1f;
    public Vector3 move;
    //public float horizontal, vertical;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rotator = GetComponent<Rotator>();
    }

    // Update is called once per frame
    void Update()
    {
        //GetInput();
        ApplyForce();
    }

    public void ApplyForce()
    {
        rigid.AddForce(move * speed);
        rigid.AddTorque(Vector3.up * additionalTorque * move.magnitude);
    }
}
