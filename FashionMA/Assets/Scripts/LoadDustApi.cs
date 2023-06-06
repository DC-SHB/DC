using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class LoadDustApi : MonoBehaviour
{

    private string jsonResult;
    private string[,] gradeResult = new string[19, 2];

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
        //string informGrade = "서울 : 보통,제주 : 좋음,전남 : 좋음,전북 : 보통,광주 : 보통,경남 : 좋음,경북 : 좋음,울산 : 좋음,대구 : 좋음,부산 : 좋음,충남 : 보통,충북 : 보통,세종 : 보통,대전 : 보통,영동 : 좋음,영서 : 좋음,경기남부 : 보통,경기북부 : 보통,인천 : 보통";
        string[] result = informGrade.Split(new char[] { ':',',',' '});
        int col = 0; int row = 0;
        for (int i = 0; i < result.Length; i++)
        {
            if(!(String.IsNullOrEmpty(result[i])))
            {
                if (col == 0)
                {
                    gradeResult[row, col] = result[i];
                    col = 1;
                }
                else
                {
                    gradeResult[row, col] = result[i];
                    col = 0;
                    row += 1;
                }
            }
        }

        //for (int i=0; i<gradeResult.GetLength(0); i++)
        //{
        //    Debug.Log("["+i+"] " + gradeResult[i, 0] + " " + gradeResult[i, 1]);
        //}
    }

    void LoadData()
    {
        StartCoroutine(GetDustData());
    }

    IEnumerator GetDustData()
    {
        string url = "http://apis.data.go.kr/B552584/ArpltnInforInqireSvc/getMinuDustFrcstDspth?serviceKey=WtsME%2FdmW6hfOBqaugjZSVpowLl%2BgVqstWO7Je8LwN62YwAC310c7eB3g20O1V7j95OPcE7Vc8B1ZRkJt7fAGg%3D%3D&returnType=json&numOfRows=5&pageNo=1&searchDate=2023-06-05&InformCode=PM10";
        UnityWebRequest www = UnityWebRequest.Get(url);
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
}
