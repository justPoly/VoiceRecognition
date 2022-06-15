using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

[Serializable]
public class MessageSender
{
    public string sender, message;
}
public class Message
{
    public string recipient_id, text, image;
}
public class HTTP : MonoBehaviour
{
    private string text;
    public Text displayIncomingText;
    public Text inputField;
    //public Text displayOutgoingText;

    public void CallPostRequest(string txt)
    {
        MessageSender thisMSG = new MessageSender();
        thisMSG.sender = "Rasa";
        // read from the input field here.
        thisMSG.message = inputField.text;
        //displayOutgoingText.text = inputField.text;
        string json = JsonUtility.ToJson(thisMSG);
        //string address = "http://localhost:5005/webhooks/rest/webhook";
        string address = "http://192.168.0.26:5005/webhooks/rest/webhook";
        StartCoroutine(PostRequest(address, json));
    }
    IEnumerator PostRequest(string uri, string json)
    {
        var uwr = new UnityWebRequest(uri, "POST");
        byte[] jsonTOSend = new System.Text.UTF8Encoding().GetBytes(json);
        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonTOSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();
        if (uwr.isNetworkError)
        {
            text = "Error";
        }
        else
        {
            text = uwr.downloadHandler.text;
            string ntext = "";
            string prev = "}";
            string now = ",";
            int prevInd = 1;
            int ind = 1;
            string empty = "" + "";
            foreach (char s in text)
            {
                if (prev == s.ToString())
                {
                    string m = text.Substring(prevInd, ind - prevInd);
                    ntext = ntext + JsonConvert.DeserializeObject<Message>(m).text;
                    Debug.Log(m);
                    prevInd = ind + 1;
                    ntext = ntext + empty;
                }
                ind = ind + 1;
            }
            int startInd = text.IndexOf("text") + 6;
            int length = text.Length - 3 - startInd;
            text = ntext;
        }
        displayIncomingText.text = text;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
