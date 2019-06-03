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
        public Bot bot;

        [Header("Rotator")]
        public bool isRotator = false;
        public float rotationLimit = 0f;

        [Header("Weapon")]
        public bool isWeapon = false;
        public BulletData bullet;
        

        public void Register(Bot _bot, GameObject _toExecuteOn)
        {

            var copy = new LogicBlock();
            copy.triggerType = triggerType;
            copy.executeOn = _toExecuteOn;
            copy.bot = _bot;
            copy.isRotator = isRotator;
            copy.rotationLimit = rotationLimit;
            copy.isWeapon = isWeapon;
            copy.bullet = bullet;

            bot = _bot;
            executeOn = _toExecuteOn;

            if (triggerType == TriggerType.Update)
            {
                _bot.OnUpdate.AddListener(copy.Execute);
            }
            if (triggerType == TriggerType.Attack)
            {
                _bot.OnAttack.AddListener(copy.Execute);
            }
            if (triggerType == TriggerType.Movement)
            {
                _bot.OnMove.AddListener(copy.Execute);
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
                //Debug.Log("This gets called how often?");
                BulletManager.singleton.InstantiateBullet(bullet, executeOn.transform.position + executeOn.transform.forward * 0.5f, executeOn.transform.rotation, bullet.damage,bot);
                //Debug.Log("We got out here atleast");
            }

        }

    }

    public List<LogicBlock> logicBlocks;

    public void Register(Bot bot, GameObject part)
    {
        foreach (var block in logicBlocks)
            block.Register(bot,part);
    }

    /*public LogicData(LogicData source)
    {
        logicBlocks = new List<LogicBlock>(source.logicBlocks);
        Debug.Log("[LogicData] source:" + source.logicBlocks.Count + " mine: " + logicBlocks.Count);
    }*/

}
