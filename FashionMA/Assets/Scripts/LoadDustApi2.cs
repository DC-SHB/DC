using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class LoadDustApi2 : MonoBehaviour
{

    private string jsonResult;

    void Start()
    {
        LoadData();
    }

    public void InitData()
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);

        string informCode = ItemData["response"]["body"]["items"][0]["informCode"].ToString(); //informCode, informData, informGrade, dataTime
        string informData = ItemData["response"]["body"]["items"][0]["informData"].ToString();
        string informGrade = ItemData["response"]["body"]["items"][0]["informGrade"].ToString();
        string dataTime = ItemData["response"]["body"]["items"][0]["dataTime"].ToString();
        Debug.Log(informCode);
        Debug.Log(informData);
        Debug.Log(informGrade);
        Debug.Log(dataTime);
    }

    void LoadData()
    {
        StartCoroutine(GetDustData());
    }

    IEnumerator GetDustData()
    {
        string url = "http://apis.data.go.kr/B552584/ArpltnInforInqireSvc/getMsrstnAcctoRltmMesureDnsty?stationName=Á¾·Î±¸&dataTerm=daily&pageNo=1&numOfRows=5&returnType=json&serviceKey=WtsME%2FdmW6hfOBqaugjZSVpowLl%2BgVqstWO7Je8LwN62YwAC310c7eB3g20O1V7j95OPcE7Vc8B1ZRkJt7fAGg%3D%3D";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            jsonResult = www.downloadHandler.text;
            InitData();
        }
    }
}
