using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotConfiguator : MonoBehaviour
{
    public BotData defaultBot;

    public GameObject activeBot;

    private void Start()
    {
        activeBot = GenerateBotFromData(defaultBot);
    }

    public void GenerateRandomBot()
    {
        Destroy(activeBot);
        BotData dat = ScriptableObject.CreateInstance<BotData>();
        List<PartData> chassis = PartDatabase.singleton.parts.FindAll(x => x.GetType().Equals(typeof(ChassisData)));
        dat.chassis = chassis[Random.Range(0, chassis.Count)] as ChassisData;

        List<PartData> wheels = PartDatabase.singleton.parts.FindAll(x => x.GetType().Equals(typeof(WheelData)));
        dat.wheels = wheels[Random.Range(0, wheels.Count)] as WheelData;

        List<PartData> misc = PartDatabase.singleton.parts.FindAll(x => x.GetType() != typeof(ChassisData) && x.GetType() != typeof(WheelData));
        dat.attachements = new List<PartData>();
        dat.attachements.Add(misc[Random.Range(0,misc.Count)]);


        defaultBot = dat;
        activeBot = GenerateBotFromData(dat);
    }

    public GameObject GenerateBotFromData(BotData bot)
    {
        GameObject newBot = new GameObject();
        newBot.transform.position = transform.position;
        newBot.AddComponent<BotBrain>();
        var script = newBot.AddComponent<Bot>();
        script.wheels = InstantiateFromPart(bot.wheels,newBot.transform);
        script.chassis = InstantiateFromPart(bot.chassis, script.wheels.transform);
        script.weapon = InstantiateFromPart(bot.attachements[0], script.chassis.transform);

        return newBot;
    }

    public GameObject InstantiateFromPart(PartData part, Transform attachedTo = null)
    {
        Transform parent = attachedTo ? attachedTo : transform;
        GameObject clone = Instantiate(part.prefab, parent);

        return clone;
    }

}
