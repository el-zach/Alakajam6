using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class BotConfiguator : MonoBehaviour
{
    public BotData myBotData;

    public GameObject activeBot;

    public BotList botList;

    private void Start()
    {
        activeBot = GenerateBotFromData(myBotData);
    }

    private void Update()
    {
        
        if (GameSync.instance.results.ContainsKey("Send Bot To Cloud"))
        {

            GameSync.instance.results.Remove("Send Bot To Cloud");
            stillWaiting = false;
        }

        if (GameSync.instance.results.ContainsKey("Get Bots"))
        {
            botList = JsonConvert.DeserializeObject<BotList>(GameSync.instance.results["Get Bots"]);
            //NumberOfActiveUsers = userList.fields.Length;
            GameSync.instance.results.Remove("Get Bots");
        }
    }

    [ContextMenu("Get Bot Data From Server")]
    public void GetAllBots()
    {
        GameSync.instance.GetData(
            jobName: "Get Bots",
            parameters: new string[] { "type=get-all", "from=bots", "db=beyblade" }
        );
    }

    public void GenerateRandomBot()
    {
        Destroy(activeBot);
        BotData dat = ScriptableObject.CreateInstance<BotData>();
        dat.botName = "New Bot" + Random.Range(1000, 9999);
        List<PartData> chassis = PartDatabase.singleton.parts.FindAll(x => x.GetType().Equals(typeof(ChassisData)));
        dat.chassis = chassis[Random.Range(0, chassis.Count)] as ChassisData;

        List<PartData> wheels = PartDatabase.singleton.parts.FindAll(x => x.GetType().Equals(typeof(WheelData)));
        dat.wheels = wheels[Random.Range(0, wheels.Count)] as WheelData;

        List<PartData> misc = PartDatabase.singleton.parts.FindAll(x => x.GetType() != typeof(ChassisData) && x.GetType() != typeof(WheelData));
        dat.attachements = new List<PartData>();
        dat.attachements.Add(misc[Random.Range(0,misc.Count)]);


        myBotData = dat;
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

    /*public BotData DataFromJSON(string json)
    {

    }*/

    public bool stillWaiting = false;
    public void SendActiveBot()
    {
        if(!stillWaiting)
            SendBotToCloud(myBotData);
    }

    public void SendBotToCloud(BotData botData)
    {
        Debug.Log(botData.botName);
        stillWaiting = true;
        if (!botData)
        {
            Debug.LogError("[SendBotToCLoud] Suddenly botdata went missing");
            return;
        }
        CreateBotRequest req = new CreateBotRequest(_fields: new NewBot(botData));
        if (req != null)
        {
            Debug.Log("[SendActiveBot] req is not null <b>:></b>");
            GameSync.instance.SendData(req, "Send Bot To Cloud");
        }
        else
        {
            Debug.LogWarning("req is null <b>:></b>");
        }
    }

    
    public class CreateBotRequest
    {
        public string type = "put-data";
        public string db = "beyblade";
        public string table = "bots";
        public NewBot fields;

        public CreateBotRequest(NewBot _fields=null)
        {
            Debug.Log("[CreateBotRequest] " + _fields.name);
            Debug.Log("[CreateBotRequest] wheelsint: " + _fields.wheels);
            Debug.Log("[CreateBotRequest] cahssisint: " + _fields.chassis);
            Debug.Log("[CreateBotRequest] attachments string: " + _fields.attachments);

            type = "put-data";

            db = "beyblade";

            table = "bots";

            fields = _fields;
        }

    }
    
    [System.Serializable]
    public class NewBot
    {
        public string name = "";
        public int wheels;
        public int chassis;
        public string attachments;

        public NewBot(BotData botData=null)
        {
            if (botData==null)
            {
                Debug.LogError("[NewBot] MISSING BOTDATA");
                return;
            }
            name = botData.botName;
            Debug.Log("[NewBot] Trying to find " + botData.wheels.name+" Found at: "+  PartDatabase.singleton.parts.FindIndex(x => x == botData.wheels));
            wheels = PartDatabase.singleton.parts.FindIndex( x=>x == botData.wheels);
            Debug.Log("[NewBot] Trying to find " + botData.chassis.name + " Found at: " + PartDatabase.singleton.parts.FindIndex(x => x == botData.chassis));
            chassis = PartDatabase.singleton.parts.FindIndex(x => x == botData.chassis);
            List<int> attachmentsInt = new List<int>();
            foreach(var part in botData.attachements)
            {
                attachmentsInt.Add(PartDatabase.singleton.parts.FindIndex(x => x == part));
            }

            attachments = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(attachmentsInt)));
            //attachments = "TODO";

        }
    }

    [System.Serializable]
    public class BotWithID : NewBot
    {
        public int id;
    }

    [System.Serializable]
    public class BotList
    {
        public BotWithID[] fields;
    }

}
