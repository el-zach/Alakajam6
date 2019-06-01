using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BotData : ScriptableObject {

    public PartData chassis, wheels;
    public List<PartData> attachements;

}
