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
        public Vector3 rotation;
        public GameObject executeOn;

        public void Register(Bot bot, GameObject _toExecuteOn)
        {
            executeOn = _toExecuteOn;

            if (triggerType == TriggerType.Update)
            {
                bot.OnUpdate.AddListener(Execute);
            }
            if (triggerType == TriggerType.Attack)
            {
                bot.OnAttack.AddListener(Execute);
            }
            if (triggerType == TriggerType.Movement)
            {
                bot.OnChase.AddListener(Execute);
            }
        }

        public void Execute()
        {
            Debug.Log(triggerType);
        }

    }

    public List<LogicBlock> logicBlocks;

    public void Register(Bot bot, GameObject part)
    {
        foreach (var block in logicBlocks)
            block.Register(bot,part);
    }

}
