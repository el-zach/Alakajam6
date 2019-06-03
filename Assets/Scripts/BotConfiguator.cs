using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class BotConfiguator : MonoBehaviour
{
    public static BotConfiguator singleton;
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else { 
            Debug.LogWarning("Too many Bot Configurators", gameObject);
            Destroy(gameObject);
        }

        //GetAllBots();
    }
    public GameObject botPrefab;

    public BotData myBotData;
    public BotData playerBotData;

    public GameObject activeBot;
    public UnityEngine.UI.Text botCountText;
    public BotList botList;

    public bool generateBotWithPlayerInput = false;
    public bool generatePreviewBot = false;

    public GameObject playerSelectionRing;

    private void Start()
    {
        //activeBot = GenerateBotFromData(myBotData,generateBotWithPlayerInput,transform);
        GetAllBots();
    }

    private void Update()
    {
        
        if (GameSync.instance.results.ContainsKey("Send Bot To Cloud"))
        {

            GameSync.instance.results.Remove("Send Bot To Cloud");
            stillWaiting = false;
        }

        if (GameSync.instance.results.ContainsKey("Update Bot In Cloud"))
        {

            GameSync.instance.results.Remove("Update Bot In Cloud");
            stillWaiting = false;
        }

        if (GameSync.instance.results.ContainsKey("Get Bots"))
        {
            botList = JsonUtility.FromJson<BotList>(GameSync.instance.results["Get Bots"]);
            // botList = JsonConvert.DeserializeObject<BotList>(GameSync.instance.results["Get Bots"]);
            //NumberOfActiveUsers = userList.fields.Length;
            GameSync.instance.results.Remove("Get Bots");
            GetBotsFromFields();
            botCountText.text = "Bots in database: " + botList.fields.Length+"  ";
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            GetAllBots();
        }
    }

    [ContextMenu("Get Bot Data From Server")]
    public void GetAllBots()
    {
        GameSync.instance.GetData(
            jobName: "Get Bots",
            parameters: new string[] { "type=get-all", "from="+GameSync.instance.tableName, "db=beyblade" }
        );
    }

    [ContextMenu("Update Bot Data at [1]")]
    public void TestUpdate()
    {
        BotData botData = myBotData;
        Debug.Log(botData.botName);
        stillWaiting = true;
        if (!botData)
        {
            Debug.LogError("[SendBotToCLoud] Suddenly botdata went missing");
            return;
        }
        var nb = new NewBot();
        nb.InitFromData(botData);
        UpdateBotRequest req = new UpdateBotRequest(1, nb);
        if (req != null)
        {
            Debug.Log("[SendActiveBot] req is not null <b>:></b>");
            GameSync.instance.SendData(req, "Update Bot In Cloud");
        }
        else
        {
            Debug.LogWarning("req is null <b>:></b>");
        }
    }


    public UnityEngine.UI.InputField nameField;
    public void NameBot()
    {
        if (!nameField)
            nameField = FindObjectOfType<UnityEngine.UI.InputField>();
        NameBot(nameField.text);
        nameField = null;
    }

    public void NameBot(string _newName)
    {
        if(_newName.Length>1)
            myBotData.botName = _newName;
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

        List<PartData> weapons = PartDatabase.singleton.parts.FindAll(x => x.GetType().Equals(typeof(WeaponData)));
        dat.weapon = weapons[Random.Range(0, weapons.Count)] as WeaponData;

        List<PartData> motors = PartDatabase.singleton.parts.FindAll(x => x.GetType().Equals(typeof(MotorData)));
        dat.motor = motors[Random.Range(0, motors.Count)] as MotorData;

        List<PartData> mantles = PartDatabase.singleton.parts.FindAll(x => x.GetType().Equals(typeof(MantleData)));
        dat.mantle = mantles[Random.Range(0, mantles.Count)] as MantleData;

        /*List<PartData> misc = PartDatabase.singleton.parts.FindAll(x => x.GetType() != typeof(ChassisData) && x.GetType() != typeof(WheelData));
        dat.attachements = new List<PartData>();
        dat.attachements.Add(misc[Random.Range(0,misc.Count)]);*/


        myBotData = dat;
        activeBot = GenerateBotFromData(dat,generateBotWithPlayerInput, generatePreviewBot,transform);
        activeBot.GetComponent<Bot>().inactive = true;
    }

    public void ColorizeRandom()
    {
        if(activeBot)
        {
            var bot = activeBot.GetComponent<Bot>();

            var s = Random.Range(0.1f, 0.5f);
            var r = Random.Range(s, 1 - s);

            var saturationMin = 0.2f;
            var saturationMax = 0.6f;

            Color randColor = Random.ColorHSV(r - s, r + s, saturationMin, saturationMax);
            bot.data.color = randColor;

            SetColor(bot.wheels, randColor);
            SetColor(bot.chassis, randColor);
            SetColor(bot.weapon, randColor);
            SetColor(bot.motor, randColor);
            SetColor(bot.mantle, randColor);
        }
    }

    public static void SetColor(GameObject go, Color c)
    {
        var prop = new MaterialPropertyBlock();
        prop.SetColor("_BaseColor", c);
        var rs = go.GetComponentsInChildren<MeshRenderer>();
        foreach(var r in rs)
        {
            r.SetPropertyBlock(prop);
        }
    }

    public static GameObject GenerateBotFromData(BotData bot, bool _playerInput, bool generatePreview, Transform spawnContainer = null)
    {
        //GameObject newBot = new GameObject();
        Debug.Log("Spawning Bot " + bot.name);
        
        GameObject newBot = Instantiate(singleton.botPrefab);
        newBot.name = bot.botName;
        if(spawnContainer)
            newBot.transform.position = spawnContainer.position;

        var bCol = newBot.AddComponent<BoxCollider>();
        bCol.center = Vector3.up;
        bCol.size = new Vector3(2.5f, 2f, 2.5f);
        if (!generatePreview)
        {
            var brain = newBot.AddComponent<BotBrain>();
            
            newBot.AddComponent<Rigidbody>();
            if (_playerInput)
            {
                newBot.AddComponent<PlayerInput>();
                brain.useBrain = false;
            }
            newBot.GetComponentInChildren<HealthBar>().nameText.text = bot.botName;
        }
        var script = newBot.AddComponent<Bot>();

        script.data = bot;
        script.wheels = InstantiateFromPart(bot.wheels, script, newBot.transform);
        script.chassis = InstantiateFromPart(bot.chassis, script, script.wheels.transform);
        script.weapon = InstantiateFromPart(bot.weapon, script, script.chassis.transform);
        script.motor = InstantiateFromPart(bot.motor, script, script.weapon.transform);
        script.mantle = InstantiateFromPart(bot.mantle, script, script.chassis.transform);

        SetColor(script.wheels, bot.color);
        SetColor(script.chassis, bot.color);
        SetColor(script.weapon, bot.color);
        SetColor(script.motor, bot.color);
        SetColor(script.mantle, bot.color);

        return newBot;
    }

    public static GameObject InstantiateFromPart(PartData part, Bot _bot, Transform attachedTo)
    {
        Transform parent = attachedTo;
        GameObject clone = Instantiate(part.prefab, parent);
        if (part.logic)
        {
            /*Debug.Log("[BotConfiguator] this is happening");
            var newLogic = ScriptableObject.CreateInstance<LogicData>();
            newLogic.logicBlocks = part.logic.logicBlocks;
            part.logic = newLogic;*/
            part.logic.Register(_bot, clone);
        }
        return clone;
    }

    public List<BotData> possibleBots;

    public void GetBotsFromFields()
    {
        possibleBots = new List<BotData>();

        foreach(var bot in botList.fields)
        {
            possibleBots.Add(BotFromString(bot));
        }

    }

    public BotData BotFromString(BotWithID stringBot)
    {
        BotData newBot = ScriptableObject.CreateInstance<BotData>();
        newBot.botName = stringBot.name;
        newBot.wheels = PartDatabase.singleton.parts[stringBot.wheels]as WheelData;
        newBot.chassis = PartDatabase.singleton.parts[stringBot.chassis] as ChassisData;
        newBot.weapon = PartDatabase.singleton.parts[stringBot.weapon] as WeaponData;
        newBot.motor = PartDatabase.singleton.parts[stringBot.motor] as MotorData;
        newBot.mantle = PartDatabase.singleton.parts[stringBot.mantle] as MantleData;
        newBot.killCount = stringBot.killCount;
        Color col;
        if (ColorUtility.TryParseHtmlString("#"+stringBot.color, out col))
        {
            newBot.color = col;
        }
        else
        {
            Debug.Log("Couldnt parse color, was: " + stringBot.color);
            newBot.color = Color.white;
        }
        return newBot;
    }

    public bool stillWaiting = false;
    public void SendActiveBot()
    {
        if(!stillWaiting)
            SendBotToCloud(myBotData);

        playerBotData.botName = myBotData.botName;
        playerBotData.chassis = myBotData.chassis;
        playerBotData.motor = myBotData.motor;
        playerBotData.mantle = myBotData.mantle;
        playerBotData.weapon = myBotData.weapon;
        playerBotData.wheels = myBotData.wheels;
        playerBotData.color = myBotData.color;
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
        var nb = new NewBot();
        nb.InitFromData(botData);
        CreateBotRequest req = new CreateBotRequest(nb);
        if (req != null)
        {
            Debug.Log("[SendActiveBot] req is not null <b>:></b>");
            GameSync.instance.SendData(req, "Send Bot To Cloud");
        }
        else
        {
            Debug.LogWarning("req is null <b>:></b>");
        }
        activeBot = null;
    }

    
    public class CreateBotRequest
    {
        public string type = "put-data";
        public string db = "beyblade";
        public string table = GameSync.instance.tableName;
        public NewBot fields;

        public CreateBotRequest(NewBot _fields=null)
        {
            Debug.Log("[CreateBotRequest] " + _fields.name);
            Debug.Log("[CreateBotRequest] wheelsint: " + _fields.wheels);
            Debug.Log("[CreateBotRequest] cahssisint: " + _fields.chassis);
            Debug.Log("[CreateBotRequest] weaponint: " + _fields.weapon);
            Debug.Log("[CreateBotRequest] motorint: " + _fields.motor);
            Debug.Log("[CreateBotRequest] mantleint: " + _fields.mantle);
            Debug.Log("[CreateBotRequest] killcountint: " + _fields.killCount);

            type = "put-data";

            db = "beyblade";

            table = GameSync.instance.tableName;

            fields = _fields;
        }

    }

    public class UpdateBotRequest
    {
        public string type = "update-data";
        public string db = "beyblade";
        public string table = GameSync.instance.tableName;
        public int id;
        public NewBot fields;

        public UpdateBotRequest(int _id, NewBot _fields = null)
        {
            Debug.Log("[UpdateBotRequest] " + _fields.name);
            Debug.Log("[UpdateBotRequest] wheelsint: " + _fields.wheels);
            Debug.Log("[UpdateBotRequest] cahssisint: " + _fields.chassis);
            Debug.Log("[UpdateBotRequest] weaponint: " + _fields.weapon);
            Debug.Log("[UpdateBotRequest] motorint: " + _fields.motor);
            Debug.Log("[UpdateBotRequest] mantleint: " + _fields.mantle);
            Debug.Log("[UpdateBotRequest] killcountint: " + _fields.killCount);

            type = "update-data";

            db = "beyblade";

            table = GameSync.instance.tableName;

            id = _id;

            fields = _fields;
        }

    }

    //create table request
    // + string createtable
    // + gleiche variablen (wie in der json) als public

    [System.Serializable]
    public class NewBot
    {
        public string name = "";
        public int wheels;
        public int chassis;
        public int weapon;
        public int motor;
        public int mantle;
        public string color;
        public int killCount=0;

        public void InitFromData(BotData botData)
        {
            if (botData == null)
            {
                Debug.LogWarning("[NewBot] MISSING BOTDATA");
                return;
            }
            name = botData.botName;
            Debug.Log("[NewBot] Trying to find " + botData.wheels.name + " Found at: " + PartDatabase.singleton.parts.FindIndex(x => x == botData.wheels));
            wheels = PartDatabase.singleton.parts.FindIndex(x => x == botData.wheels);
            Debug.Log("[NewBot] Trying to find " + botData.chassis.name + " Found at: " + PartDatabase.singleton.parts.FindIndex(x => x == botData.chassis));
            chassis = PartDatabase.singleton.parts.FindIndex(x => x == botData.chassis);

            Debug.Log("[NewBot] Trying to find " + botData.weapon.name + " Found at: " + PartDatabase.singleton.parts.FindIndex(x => x == botData.weapon));
            weapon = PartDatabase.singleton.parts.FindIndex(x => x == botData.weapon);
            Debug.Log("[NewBot] Trying to find " + botData.motor.name + " Found at: " + PartDatabase.singleton.parts.FindIndex(x => x == botData.motor));
            motor = PartDatabase.singleton.parts.FindIndex(x => x == botData.motor);
            Debug.Log("[NewBot] Trying to find " + botData.mantle.name + " Found at: " + PartDatabase.singleton.parts.FindIndex(x => x == botData.mantle));
            mantle = PartDatabase.singleton.parts.FindIndex(x => x == botData.mantle);

            killCount = botData.killCount;
            color = ColorUtility.ToHtmlStringRGB(botData.color);

            /*List<int> attachmentsInt = new List<int>();
            foreach(var part in botData.attachements)
            {
                attachmentsInt.Add(PartDatabase.singleton.parts.FindIndex(x => x == part));
            }

            attachments = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(attachmentsInt)));*/
            //attachments = "TODO";

        }

        public NewBot()
        {
            
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
