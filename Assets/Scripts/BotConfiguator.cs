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
        var wheels = InstantiateFromPart(bot.wheels);
        var chassis = InstantiateFromPart(bot.chassis, wheels.transform);
        InstantiateFromPart(bot.attachements[0], chassis.transform);
    }

    public GameObject InstantiateFromPart(PartData part, Transform attachedTo = null)
    {
        Transform parent = attachedTo ? attachedTo : transform;
        GameObject clone = Instantiate(part.prefab, parent);

        return clone;
    }

}
