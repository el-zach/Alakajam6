using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    Rigidbody rigid;
    public float startTorque = 30f;
    public float contiousTorque = 3f;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.maxAngularVelocity = Mathf.Infinity;
        rigid.AddTorque(Vector3.up * startTorque);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigid.AddTorque(Vector3.up * contiousTorque);
    }
}
