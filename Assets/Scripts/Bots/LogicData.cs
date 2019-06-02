using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LogicData : ScriptableObject
{
    public enum TriggerType { Attack, Movement, Update}


    [System.Serializable]
    public class LogicBlock
    {
        public TriggerType triggerType;
        public GameObject executeOn;
        Bot bot;

        [Header("Rotator")]
        public bool isRotator = false;
        public float rotationLimit = 0f;

        [Header("Weapon")]
        public bool isWeapon = false;
        public BulletData bullet;



        public void Register(Bot _bot, GameObject _toExecuteOn)
        {
            bot = _bot;
            executeOn = _toExecuteOn;

            if (triggerType == TriggerType.Update)
            {
                _bot.OnUpdate.AddListener(Execute);
            }
            if (triggerType == TriggerType.Attack)
            {
                _bot.OnAttack.AddListener(Execute);
            }
            if (triggerType == TriggerType.Movement)
            {
                _bot.OnMove.AddListener(Execute);
            }
        }

        public void Execute()
        {
            if (isRotator)
            {
                if (rotationLimit == 0f)
                    executeOn.transform.Rotate(Vector3.up * bot.rotationalSpeed * Time.deltaTime);
                else
                {
                    executeOn.transform.localRotation = Quaternion.Euler(Vector3.up * Mathf.Sin(Time.time * bot.rotationalSpeed) * rotationLimit);
                }
            }

            if (isWeapon)
            {
                BulletManager.singleton.InstantiateBullet(bullet, executeOn.transform.position + executeOn.transform.forward * 0.5f, executeOn.transform.rotation, bullet.damage,bot);
            }

        }

    }

    public List<LogicBlock> logicBlocks;

    public void Register(Bot bot, GameObject part)
    {
        foreach (var block in logicBlocks)
            block.Register(bot,part);
    }

}
