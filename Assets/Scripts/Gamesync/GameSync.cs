using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using System.Text;
using System;
using Newtonsoft.Json;

public class GameSync : MonoBehaviour
{
    public static GameSync instance;

    void Awake () {
        if (instance != null) {
            Destroy(this.gameObject);
        } else {
            instance = this;
        }
    }

    private string Server = @"https://gamesync.fumiko-game.com";

    public Dictionary<string, string> results = new Dictionary<string, string>();

    [System.Serializable]
    class InternalErrorMessage {
        [SerializeField]
        string type = "internal error";
        [SerializeField]
        string message = "";

        public InternalErrorMessage (string Message) {
            message = Message;
        }
    }

    class Result {
        public string type = "";
        public string message = "";
        public string fields = "";
    }

    void Start () {
    }

    public void GetData (string [] parameters, string jobName) {
        StartCoroutine(_GetData(jobName, parameters));
    }

    public void SendData (object blob, string jobName) {
        StartCoroutine(_SendData(blob, jobName));
    }

    IEnumerator _SendData(object blob, string jobName)
    {
        WWWForm content = new WWWForm();
        string jsonTest = JsonConvert.SerializeObject(blob);
        Debug.Log(jsonTest);
        string blobJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(blob)));
        content.AddField("content", blobJson);

        UnityWebRequest request = UnityWebRequest.Post(Server, content);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            results.Add(jobName, JsonConvert.SerializeObject(new InternalErrorMessage(request.error)));
        }
        else
        {
            results.Add(jobName, request.downloadHandler.text);
        }
    }

    IEnumerator _GetData (string jobName, string[] _parameters) {
        string parameters = "";

        if (_parameters.Length > 0) {
            for (int i = 0; i < _parameters.Length; i++) {
                parameters += _parameters[i] + "&";
            }

            parameters = parameters.Substring(0, parameters.Length - 1);
        }

        UnityWebRequest request = UnityWebRequest.Get(Server + "?" + parameters.ToString());

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            results.Add(jobName, JsonConvert.SerializeObject(new InternalErrorMessage(request.error)));
        }
        else
        {
            results.Add(jobName, request.downloadHandler.text);
        }
    }
}
