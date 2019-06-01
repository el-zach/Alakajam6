using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotConfiguator : MonoBehaviour
{
    public BotData defaultBot;

    private void Start()
    {
        GenerateBotFromData(defaultBot);
    }

    public void GenerateBotFromData(BotData bot)
    {
        GameObject newBot = new GameObject();
        newBot.transform.position = transform.position;
        newBot.AddComponent<BotBrain>();
        var script = newBot.AddComponent<Bot>();
        script.wheels = InstantiateFromPart(bot.wheels,newBot.transform);
        script.chassis = InstantiateFromPart(bot.chassis, script.wheels.transform);
        script.weapon = InstantiateFromPart(bot.attachements[0], script.chassis.transform);
    }

    public GameObject InstantiateFromPart(PartData part, Transform attachedTo = null)
    {
        Transform parent = attachedTo ? attachedTo : transform;
        GameObject clone = Instantiate(part.prefab, parent);

        return clone;
    }

}
