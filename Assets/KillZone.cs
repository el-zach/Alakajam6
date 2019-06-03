using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var script = other.GetComponent<Health>();
        if (script)
        {
            script.Death();
        }
    }
}
