using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bot : MonoBehaviour
{
    [System.Serializable]
    public class Event : UnityEvent<Bot> { }

    [Header("Stats")]
    public float maxHealth=10f;
    public float weight=1f;
    public AnimationCurve spurtCurve;
    public float speed=1f;
    public float spurtDuration=1f;
    public float spurtCooldown=0.2f;

    public float rotationalSpeed=1f;

    public float fireRate=3f;
    public int salveCount=5;
    public float salveCooldown = 2f;

    [Header("Runtime Properties")]
    public Vector3 movementInput;
    public Transform aimingAt;
    public float health;

    public float currentSpurt;
    public float currentSpurtCooldown;

    public float shotCooldown;
    public int shots;
    public float currentSalveCooldown;

    Quaternion startRotation;
    public Quaternion targetRotation;

    [Header("System stuff")]
    public BotData data;
    public GameObject wheels, chassis, weapon, motor, mantle;
    public UnityEvent OnAttack= new UnityEvent(), OnMove = new UnityEvent(), OnUpdate = new UnityEvent();
    Rigidbody rigid;
    public List<Collider> toExclude=new List<Collider>();


    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        toExclude.Add(GetComponent<Collider>());
        SetupFromBotData();
    }

    void SetupFromBotData()
    {
        //-----wheels-----//
        spurtCurve = data.wheels.spurtCurve;
        speed = data.wheels.speed;
        spurtDuration = data.wheels.spurtDuration;
        currentSpurt = spurtDuration;
        spurtCooldown = data.wheels.spurtCooldown;

        //-----weapon------//
        fireRate = data.weapon.fireRate;
        salveCount = data.weapon.salveCount;
        salveCooldown = data.weapon.salveCooldown;

        //----chassis---//
        rotationalSpeed = data.chassis.rotationalSpeed;
        fireRate *= data.chassis.fireRateBonus;
        salveCount = Mathf.FloorToInt(salveCount * data.chassis.salveCountBonus);
        salveCooldown *= data.chassis.salveCooldownBonus;

        shots = salveCount;
    }

    private void Update()
    {
        OnUpdate.Invoke();
        Move();
        ComputeMovement();
        Shooting();
    }

    void Shooting()
    {

        currentSalveCooldown += Time.deltaTime;

        if(currentSalveCooldown >= salveCooldown)
        {
            if( shotCooldown >= 1f/fireRate)
            {
                Shot();
                shotCooldown = 0f;
                shots++;
            }
            else
            {
                shotCooldown += Time.deltaTime;
            }
            if (shots >= salveCount)
            {
                currentSalveCooldown = 0f;
                shots = 0;
            }
        }
    }

    void Shot()
    {
        //Debug.Log("[Bot] Shoot!",gameObject);
        OnAttack.Invoke();
    }

    private void Move()
    {
        if (currentSpurt >= spurtDuration)
            currentSpurtCooldown += Time.deltaTime;
        else
            currentSpurtCooldown = 0f;

        if (currentSpurtCooldown >= spurtCooldown)
        {
            if (movementInput != Vector3.zero)
            {
                Turn();
                Spurt();
            }
            movementInput = Vector3.zero;
        }
    }

    void Turn()
    {
        //rigid.MoveRotation(Quaternion.LookRotation(Vector3.Lerp(transform.forward, movementInput.normalized, 0.5f)));
        //rigid.MoveRotation(Quaternion.LookRotation(movementInput.normalized));
        targetRotation = Quaternion.LookRotation(movementInput.normalized);
        startRotation = transform.rotation;
    }

    void Spurt()
    {
        currentSpurt = 0f;
    }
    
    void ComputeMovement()
    {
        if (currentSpurt >= spurtDuration)
            return;

        currentSpurt += Time.deltaTime;
        Vector3 direction = transform.forward;
        float velocity = spurtCurve.Evaluate(currentSpurt / spurtDuration) * speed * Time.deltaTime;

        rigid.MovePosition(rigid.position + direction * velocity);
        rigid.MoveRotation(Quaternion.Lerp(startRotation,targetRotation,2f*currentSpurt/spurtDuration));
    }

    public void DeRegisterBulletCollider(Bullet bullet)
    {
        toExclude.Remove(bullet.myCollider);
    }


}
