using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDatabase : MonoBehaviour
{

    public static PartDatabase singleton;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
        {
            Debug.LogWarning("PartDatabase already exists!", this);
        }
    }

    public List<PartData> parts;
}
