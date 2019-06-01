using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BotData : ScriptableObject {

    public ChassisData chassis;
    public WheelData wheels;
    public List<PartData> attachements;

}
