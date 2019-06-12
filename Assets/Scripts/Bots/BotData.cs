using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class BotData : ScriptableObject {

    public string botName;
    public WheelData wheels;
    public WeaponData weapon;
    public ChassisData chassis;
    public MotorData motor;
    public MantleData mantle;
    public Color color=Color.white;
    [Header("Dynamic")]
    public int killCount;
}
