using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GoogleService : MonoBehaviour
{
    public PicFactory picFactory;
    public VoiceController voiceController;
    //public Button button;
    //Text uiText;
    private const string API_KEY = "AIzaSyBjfQaG3SV7d9y8RNjKqRkFGNEHh5iOMgA";

    public void GetPicture()
    {
        //CYJu StartCoroutine(PictureRoutine());
    }
    IEnumerator PictureRoutine()
    {
        //uiText.transform.parent.gameObject.SetActive(true);
        string query = voiceController.robotWork;
        // query = WWW.EscapeURL(query + "Voice");
        query = UnityWebRequest.EscapeURL(query + "Voice");
        picFactory.DeleteOldPictures();
        Vector3 cameraForward = Camera.main.transform.forward;

        int rowNum = 1;
        for (int i = 1; i <= 60; i += 10)
        {
            string url = "https://customsearch.googleapis.com/customsearch/v1?cx=016406801227351871376%3Axwemqsi-qci&filter=1&num=10&q=" + query +
            "&searchType=image&start=" + i + "1&fields=items%2Flink&key=" + API_KEY;
            //WWW www = new WWW(url);
            UnityWebRequest www = new UnityWebRequest();
            www = UnityWebRequest.Get(url);
            //add
            www.downloadHandler = new DownloadHandlerBuffer();

            //yield return www;
            yield return www.SendWebRequest();

            //picFactory.CreateImages(ParseResponse(www.text), rowNum, cameraForward);
            //picFactory.CreateImages(ParseResponse(button.GetComponentInChildren<Text>().text), rowNum, cameraForward);
            picFactory.CreateImages(ParseResponse(www.downloadHandler.text), rowNum, cameraForward);
            rowNum++;
        }
        yield return new WaitForSeconds(5f);
        //uiText.transform.parent.gameObject.SetActive(true);
    }
    List<string> ParseResponse(string text)
    {
        List<string> urlList = new List<string>();
        string[] urls = text.Split('\n');
        foreach (string line in urls)
        {
            if (line.Contains("link"))
            {
                string url = line.Substring(12, line.Length - 13);
                if (url.Contains(".jpg") || url.Contains(".png"))
                {
                    urlList.Add(url);
                }
            }
        }
        return urlList;
    }
}
