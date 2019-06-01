using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PartData : ScriptableObject
{
    public GameObject prefab;

    public PartData attachedTo;
    public Vector3 attachmentPoint;

    public float weight;
    public float maxHealth;

    public LogicData logic;
}
