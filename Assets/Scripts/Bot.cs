using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEvent<Bot> { }

    [Header("Stats")]
    public float maxHealth;
    public float weight;
    public float speed;
    public float spurtDuration;
    public float spurtCooldown;
    public float fireRate;
    public int salveCount;

    [Header("Runtime Properties")]
    public Vector3 movementInput;
    public Transform aimingAt;
    public float health;
    public float shotCooldown;
    public int shots;
    public float salveCooldown;

    [Header("System stuff")]
    public BotData data;
    public GameObject wheels, chassis, weapon, motor, mantle;
    public UnityEvent OnAttack= new UnityEvent(), OnMove = new UnityEvent(), OnUpdate = new UnityEvent();


    

    private void Update()
    {
        OnUpdate.Invoke();
    }


}
