using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using LitJson;

public class GetLocation : MonoBehaviour
{
    private string strBaseURL = "https://naveropenapi.apigw.ntruss.com/map-reversegeocode/v2/gc";
    private string latitude = "";
    private string longitude = "";
    private string strAPIKey = "9o2m6mrc31";
    private string secretKey = "Q83Y9IqLhwkd0sxMZQWySb8c8CgCHUcVNHtmJXKh";

    private string jsonResult;
    private string[] locationResult = new string[3];

    private bool GPS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitData()
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);

        locationResult[0] = ItemData["results"][0]["region"]["area1"]["name"].ToString();
        locationResult[1] = ItemData["results"][0]["region"]["area2"]["name"].ToString();
        locationResult[2] = ItemData["results"][0]["region"]["area3"]["name"].ToString();

        UniteData.location0 = locationResult[0];
        UniteData.location1 = locationResult[1];
        UniteData.location2 = locationResult[2];

        //for (int i=0; i< locationResult.Length; i++)
        //{
        //    Debug.Log("["+i+"] " + locationResult[i]);
        //}
    }

    IEnumerator MapLoader()
    {
        string url = strBaseURL + "?coords=" + longitude + "," + latitude + "&output=json";
        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", strAPIKey);
        www.SetRequestHeader("X-NCP-APIGW-API-KEY", secretKey);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
            jsonResult = www.downloadHandler.text;
            InitData();
        }
    }

    void Update()
    {
        if (UniteData.longitude != 0 && UniteData.latitude != 0)
        {
            GPS = true;
        }

        if(GPS)
        {
            latitude = UniteData.latitude.ToString();
            longitude = UniteData.longitude.ToString();
            StartCoroutine(MapLoader());
            GPS = false;
        }
    }
}