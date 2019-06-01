using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LogicData : ScriptableObject
{
    [System.Serializable]
    public class LogicBlock
    {

        public Vector3 rotation;

        public void Execute(PartData part)
        {

        }

    }

    public List<LogicBlock> logicBlocks;

    public void Execute(PartData part)
    {
        foreach(var block in logicBlocks)
        {
            block.Execute(part);
        }
    }

}
