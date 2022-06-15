using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PicBehavior : MonoBehaviour
{
    public Renderer quadRenderer;
    private Vector3 desiredPosition;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(Camera.main.transform);
        Vector3 desiredAngle = new Vector3(0, transform.localEulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(desiredAngle);
        //force into air
        desiredPosition = transform.localPosition;
        transform.localPosition += new Vector3(0, 20, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredPosition, Time.deltaTime * 4f);
    }

    public void LoadImage(string url)
    {
        StartCoroutine(LoadImageFormURL(url));
    }
    IEnumerator LoadImageFormURL(string url)
    {

        UnityWebRequest www = new UnityWebRequest();
        www = UnityWebRequestTexture.GetTexture(url);

        //UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            quadRenderer.material.mainTexture = DownloadHandlerTexture.GetContent(www);
        }

        /*
            WWW www = new WWW(url);
            yield return www;
            quadRenderer.material.mainTexture = www.texture;
        */
    }
}
