﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth=12f;

    [Header("Runtime")]
    public float currentHealth;
    public UnityEvent OnDeath, OnDamage;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Damage(float _damage)
    {
        currentHealth -= _damage;
        if (currentHealth <= 0f)
        {
            Death();
        }
        OnDamage.Invoke();
    }

    public void Death()
    {
        /*if (gameObject.CompareTag("Player"))
        {
            //DeathManager.singleton.currentPlayerData = pB.data;
            gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }*/

        OnDeath.Invoke();
    }


    private void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.attachedRigidbody?.CompareTag("Bullet")??false)
            {
                Damage(contact.otherCollider.attachedRigidbody.GetComponent<Bullet>().data.damage);
                //Debug.Log(gameObject.name + " was dealt damage by " + contact.otherCollider.attachedRigidbody.gameObject.name);
            }
        }
    }

}