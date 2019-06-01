using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Users : MonoBehaviour
{
    public class NewUser
    {
        public string name = "";
        public int age;
        public int score;

        public NewUser(string _name = "", int _age = 0, int _score = 0)
        {
            name = _name;
            age = _age;
            score = _score;
        }
    }

    [System.Serializable]
    public class User : NewUser {
        public int id;
    }

    public class CreateUserRequest
    {
        [SerializeField]
        string type = "put-data";
        [SerializeField]
        string db = "beyblade";
        [SerializeField]
        string table = "users";
        [SerializeField]
        NewUser fields = new NewUser();

        public CreateUserRequest(string _type = "", string _db = "", string _table = "", NewUser _fields = null)
        {
            type = _type;
            db = _db;
            table = _table;
            fields = _fields;
        }
    }

    [System.Serializable]
    public class UserList
    {
        public User[] fields;
    }

    static float UpdateInterval = 0.1f;

    public float UpdateUserListEverySeconds = 5;
    public float NumberOfActiveUsers = 0;
    public UserList userList;

    void GetAllUsers()
    {
        GameSync.instance.GetData(
            jobName: "Get Users",
            parameters: new string[] { "type=get-all", "from=users", "db=beyblade" }
        );
    }

    void CreateRandomUser()
    {
        CreateUserRequest randomUser = new CreateUserRequest(
            _type: "put-data",
            _db: "beyblade",
            _table: "users",
            _fields: new NewUser(
                _name: "New User " + UnityEngine.Random.Range(1000, 9999),
                _age: UnityEngine.Random.Range(1000, 9999),
                _score: UnityEngine.Random.Range(1000, 9999)
            )
        );

        GameSync.instance.SendData(randomUser, "Create Random User");
    }

    void Start () {
        CreateRandomUser();
    }

    float currentUpdateUserListInterval = 9999;
    void UpdateUserList () {
        if (GameSync.instance.results.ContainsKey("Get Users")) {
            userList = JsonConvert.DeserializeObject<UserList>(GameSync.instance.results["Get Users"]);
            NumberOfActiveUsers = userList.fields.Length;
            GameSync.instance.results.Remove("Get Users");
        }

        if (currentUpdateUserListInterval < UpdateUserListEverySeconds)
        {
            currentUpdateUserListInterval += UpdateInterval;
            return;
        } else {
            currentUpdateUserListInterval = 0;
        }

        GetAllUsers();
    }

    float currentUpdateInterval = UpdateInterval;
    void Update()
    {
        if (currentUpdateInterval < UpdateInterval) {
            currentUpdateInterval += Time.deltaTime;
            return;
        } else {
            currentUpdateInterval = 0;
        }

        UpdateUserList();
    }
}