using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsing : MonoBehaviour
{
    public float baseSize=1f;
    public float variableSize=0.2f;
    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        float size = baseSize + Mathf.Sin(Time.time * speed) * variableSize;
        transform.localScale = new Vector3(size,1f,size);
    }
}
