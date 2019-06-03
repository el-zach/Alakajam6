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

    public float damageMultiplier_Rock = 1f;
    public float damageMultiplier_Paper = 1f;
    public float damageMultiplier_Scissors = 1f;

    public AnimationCurve spurtCurve;
    public float speed=1f;
    public float spurtDuration=1f;
    public float spurtCooldown=0.2f;

    public float rotationalSpeed=1f;

    public float damage = 1f;
    public float fireRate=3f;
    public int salveCount=5;
    public float salveCooldown = 2f;

    [Header("Runtime Properties")]
    public bool inactive = false;
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
    public Collider myColider;
    public List<Collider> toExclude=new List<Collider>();


    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        myColider = GetComponent<Collider>();
        toExclude.Add(myColider);
        SetupFromBotData();
    }

    void SetupFromBotData()
    {
        weight = 0f;
        maxHealth = 0f;


        //-----wheels-----//
        spurtCurve = data.wheels.spurtCurve;
        speed = data.wheels.speed;
        spurtDuration = data.wheels.spurtDuration;
        currentSpurt = spurtDuration;
        spurtCooldown = data.wheels.spurtCooldown;

        weight += data.wheels.weight;
        maxHealth += data.wheels.maxHealth;

        //-----weapon------//
        fireRate = data.weapon.fireRate;
        salveCount = data.weapon.salveCount;
        salveCooldown = data.weapon.salveCooldown;
        damage = data.weapon.logic.logicBlocks[0].bullet.damage;

        weight += data.weapon.weight;
        maxHealth += data.weapon.maxHealth;

        //----chassis---//
        rotationalSpeed = data.chassis.rotationalSpeed;
        fireRate *= data.chassis.fireRateBonus;
        salveCount = Mathf.FloorToInt(salveCount * data.chassis.salveCountBonus);
        salveCooldown *= data.chassis.salveCooldownBonus;

        weight += data.chassis.weight;
        maxHealth += data.chassis.maxHealth;

        //-----Motor---//
        data.motor.ApplyMotorMultiplier(this);

        weight += data.motor.weight;
        maxHealth += data.motor.maxHealth;

        //----Mantle---//
        data.mantle.ApplyMultiplier(this);

        weight += data.mantle.weight;
        maxHealth += data.mantle.maxHealth;

        if(rigid)
            rigid.mass = weight;

        shots = salveCount;
        currentSalveCooldown = Random.Range(0f, salveCooldown);
        currentSpurtCooldown = Random.Range(0f, spurtCooldown);
    }

    private void Update()
    {
        OnUpdate.Invoke();
        if (!inactive)
        {
            Move();
            ComputeMovement();
        }
        Shooting();
    }

    void Shooting()
    {
        //Debug.Log("This should be called once per frame: "+Time.frameCount);
        currentSalveCooldown += Time.deltaTime;

        if(currentSalveCooldown >= salveCooldown)
        {
            if( shotCooldown >= 1f/fireRate)
            {
                //Debug.Log("cooldown firerate: "+shotCooldown + "/" + (1f / fireRate));
                Shot();
                //Debug.Log("Do we get here?");
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
        //Debug.Log("We invoked the shot");
    }

    private void Move()
    {
        if (currentSpurt >= spurtDuration)
            currentSpurtCooldown += Time.deltaTime;
        else
            currentSpurtCooldown = 0f;

        if (currentSpurtCooldown >= spurtCooldown)
        {
            if (movementInput.sqrMagnitude > 0.1f)
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
        if (movementInput != Vector3.zero)
            targetRotation = Quaternion.LookRotation(movementInput.normalized);
        else
            targetRotation = transform.rotation;
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
        if (targetRotation.x == 0 && targetRotation.y == 0 && targetRotation.z == 0 && targetRotation.w == 0)
            return;
        var newRot = Quaternion.Lerp(startRotation, targetRotation, Mathf.Clamp01(2f * currentSpurt / spurtDuration));
        if (!float.IsNaN(newRot.x) && !float.IsNaN(newRot.y) && !float.IsNaN(newRot.z) && !float.IsNaN(newRot.w))
            rigid.MoveRotation(newRot);
    }

    public void DeRegisterBulletCollider(Bullet bullet)
    {
        //Debug.Log("[BulletDeRegister] got called");
        Physics.IgnoreCollision(myColider, bullet.myCollider,false);
        toExclude.Remove(bullet.myCollider);
    }


}
