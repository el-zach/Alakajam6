using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEvent<Bot> { }

    public float maxHealth=12f;
    public Bot bot;

    [Header("Runtime")]
    public float currentHealth;
    public Event OnDeath;
    public UnityEvent OnDamage;

    private void Start()
    {
        currentHealth = maxHealth;
        bot = GetComponent<Bot>();
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

        OnDeath.Invoke(bot);
        gameObject.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
        foreach(ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.attachedRigidbody?.CompareTag("Bullet")??false)
            {
                Bullet bulletScript = contact.otherCollider.attachedRigidbody.GetComponent<Bullet>();
                Damage(DamageCalculation(bulletScript));
                //bot.toExclude.Add(contact.otherCollider);
                Physics.IgnoreCollision(bot.myColider, contact.otherCollider);
                bulletScript.OnBulletDespawn.AddListener(UnIgnoreCollision);

                //Debug.Log(gameObject.name + " was dealt damage by " + contact.otherCollider.attachedRigidbody.gameObject.name);
            }
        }
    }

    void UnIgnoreCollision(Bullet bullet)
    {
        Physics.IgnoreCollision(bot.myColider, bullet.myCollider, false);
    }

    float DamageCalculation(Bullet bullet)
    {
        float damage = bullet.data.damage;
        switch (bullet.data.damageType)
        {
            case DamageType.Rock:
                damage *= bot.damageMultiplier_Rock;
                break;
            case DamageType.Paper:
                damage *= bot.damageMultiplier_Paper;
                break;
            case DamageType.Scissors:
                damage *= bot.damageMultiplier_Scissors;
                break;
        }

        return damage;
    }

}
