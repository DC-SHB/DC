using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegionWeather : MonoBehaviour
{
    [SerializeField] private Sprite[] icons; // 맑음, 비, 비&눈, 눈
    [SerializeField] private GameObject[] panels;

    private GameObject[] weatherIcon;
    private GameObject[] text_temp;

    string jsonResult;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            weatherIcon[i] = panels[i].gameObject.transform.GetChild(2).gameObject;
            text_temp[i] = panels[i].gameObject.transform.GetChild(0).gameObject;
        }

        UpdateRegionWeather();
    }

    // Update is called once per frame
    void Update()
    {
                
    }

    private void UpdateRegionWeather()
    {
        // 서울(60, 127), 춘천(73, 134), 강릉(92, 131), 인천(55, 124), 수원(60, 121),
        // 대전(67, 100), 청주(69, 106), 안동(91, 106), 전주(63, 89), 창원(99, 77),
        // 대구(89, 90), 울산(102, 84), 부산(98, 76), 광주(58, 74), 목포(50, 67), 제주(52, 38), 울릉(127, 127)


        int[,] coordinate =
        {
            {60, 127 }, {73, 134}, {92, 131 },{ 55, 124},{60, 121 },
            { 67, 100},{ 69, 106},{ 91, 106},{ 63, 89},{99, 77 },
            {89, 90 }, {102, 84 },{ 98, 76},{ 58, 74},{ 50, 67},{52, 38 },{127, 127 } 
        };

        for(int i = 0; i< panels.Length; i++)
        {
            StartCoroutine(GetWeatherDataAll(1, coordinate[i, 0], coordinate[i, 1]));
        }
    }

    IEnumerator GetWeatherDataAll(int regionNum, float nx, float ny)
    {
        // 기본값 지정
        Debug.Log("data : " + UniteData.base_date + " time " + UniteData.base_time_s);

        
        string apiUrl = $"{UniteData.forecastUrl_srtNcst}?serviceKey={UniteData.apiKey}&dataType=JSON&base_date={UniteData.base_date}&base_time={UniteData.base_time_s}&nx={nx}&ny={ny}";
        //Debug.Log(apiUrl);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                
                jsonResult = webRequest.downloadHandler.text;
                InitDataGetUltraSrtNcst(jsonResult, regionNum);
                //Debug.Log(jsonResult);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
        Debug.Log("날씨 API 설정 완료");
    }
    public void InitDataGetUltraSrtNcst(string jsonResult, int regionNum)
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);
        JsonData items = ItemData["response"]["body"]["items"]["item"];

        float temp;
        int pty;

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];

            if ((string)item["category"] == "T1H") // 기온(T1H)
            {
                temp = float.Parse((string)item["obsrValue"]);
                text_temp[regionNum].GetComponent<Text>().text= temp.ToString();
                Debug.Log("기온 : " + temp);
            }

            // 맑음, 비, 비&눈, 눈
            if ((string)item["category"] == "PTY") // 강수 형태(PTY) : 없음(0) / 비(1) / 비&눈(2) / 눈(3) / 빗방울(5) / 빗방울눈날림(6) / 눈날림(7)
            {
                pty = int.Parse((string)item["obsrValue"]);

                if (pty == 0) // 강수형태 0
                {
                    weatherIcon[regionNum].GetComponent<Image>().sprite = icons[0];
                }
                else if(pty == 1 || pty == 4 || pty == 5)
                {
                    weatherIcon[regionNum].GetComponent<Image>().sprite = icons[1];
                }
                else if(pty == 2 || pty == 6)
                {
                    weatherIcon[regionNum].GetComponent<Image>().sprite = icons[2];
                }
                else if( pty == 3 || pty == 7)
                {
                    weatherIcon[regionNum].GetComponent<Image>().sprite = icons[3];
                }

                Debug.Log("강수 형태 : " + pty);
            }

        }

    }
}
