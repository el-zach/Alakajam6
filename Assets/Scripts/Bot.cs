using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEvent<Bot> { }

    public BotData data;

    public GameObject wheels, chassis, weapon;

    public UnityEvent OnAttack= new UnityEvent(), OnHealthLow = new UnityEvent(), OnChase = new UnityEvent(), OnUpdate = new UnityEvent();

    private void Update()
    {
        OnUpdate.Invoke();
    }


}
