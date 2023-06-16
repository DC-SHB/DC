using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RegionWeather : MonoBehaviour
{
    [SerializeField] private Sprite[] icons; // ����, ��, ��&��, ��
    [SerializeField] private GameObject[] panels;

    [SerializeField] private GameObject[] weatherIcon;
    [SerializeField] private GameObject[] text_temp;

    string jsonResult;

    private float[] region_temp;
    private int[] region_weather;

    private bool REGION = false;

    private int check = 0;

    // Start is called before the first frame update
    void Start()
    {
        REGION = false;

        /*
        for (int i = 0; i < panels.Length; i++)
        {
            weatherIcon[i] = panels[i].gameObject.transform.GetChild(2).gameObject;
            text_temp[i] = panels[i].gameObject.transform.GetChild(0).gameObject;
        }
        */
        region_temp = new float[panels.Length];
        region_weather = new int[panels.Length];

        check = 0;

        UpdateRegionWeather();
    }

    // Update is called once per frame
    void Update()
    {
        if (REGION)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                Debug.Log("�� : " + region_temp[i]);
                text_temp[i].GetComponent<Text>().text = region_temp[i] + "";

                switch (region_weather[i]) // ���� ����(PTY) : ����(0) / ��(1) / ��&��(2) / ��(3) / �����(5) / ����ﴫ����(6) / ������(7)
                {
                    case 0:
                        weatherIcon[i].GetComponent<Image>().sprite = icons[0];
                        break;
                    case 1:
                        weatherIcon[i].GetComponent<Image>().sprite = icons[1];
                        break;
                    case 2:
                        weatherIcon[i].GetComponent<Image>().sprite = icons[2];
                        break;
                    case 3:
                        weatherIcon[i].GetComponent<Image>().sprite = icons[3];
                        break;
                }
                
            }
            REGION = false;
        }
    }

    private void UpdateRegionWeather()
    {
        // ����(60, 127), ��õ(73, 134), ����(92, 131), ��õ(55, 124), ����(60, 121),
        // ����(67, 100), û��(69, 106), �ȵ�(91, 106), ����(63, 89), â��(99, 77),
        // �뱸(89, 90), ���(102, 84), �λ�(98, 76), ����(58, 74), ����(50, 67), ����(52, 38), �︪(127, 127)


        int[,] coordinate =
        {
            {60, 127 }, {73, 134}, {92, 131 },{ 55, 124},{60, 121 },
            { 67, 100},{ 91, 106},{ 63, 89},
            {89, 90 }, {102, 84 },{ 98, 76},{ 58, 74},{52, 38 },{127, 127 } 
        };

        int i = 0;
        for(i = 0; i< panels.Length; i++)
        {
            StartCoroutine(GetWeatherDataAll(i, coordinate[i, 0], coordinate[i, 1]));
        }
    }

    IEnumerator GetWeatherDataAll(int regionNum, float nx, float ny)
    {
        // �⺻�� ����
        Debug.Log("data : " + UniteData.base_date + " time " + UniteData.base_time_s);

        
        string apiUrl = $"{UniteData.forecastUrl_srtNcst}?serviceKey={UniteData.apiKey}&dataType=JSON&base_date={UniteData.base_date}&base_time={UniteData.base_time_s}&nx={nx}&ny={ny}";
        //Debug.Log(apiUrl);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                
                jsonResult = webRequest.downloadHandler.text;
                InitDataGetUltraSrtNcst(regionNum, jsonResult);
                //Debug.Log(jsonResult);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
        Debug.Log("���� API ���� �Ϸ�");
    }

    public void InitDataGetUltraSrtNcst(int regionNum, string jsonResult)
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);
        JsonData items = ItemData["response"]["body"]["items"]["item"];

        float temp;
        int pty;

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];

            if ((string)item["category"] == "T1H") // ���(T1H)
            {
                temp = float.Parse((string)item["obsrValue"]);
                region_temp[regionNum] = temp;
                Debug.Log("��� : " + temp);
                check++;

                if (check == panels.Length)
                    REGION = true;
            }

            // ����, ��, ��&��, ��
            else if ((string)item["category"] == "PTY") // ���� ����(PTY) : ����(0) / ��(1) / ��&��(2) / ��(3) / �����(5) / ����ﴫ����(6) / ������(7)
            {
                pty = int.Parse((string)item["obsrValue"]);

                if (pty == 0) // �������� 0
                {
                    region_weather[regionNum] = 0;
                }
                else if(pty == 1 || pty == 4 || pty == 5)
                {
                    region_weather[regionNum] = 1;
                }
                else if(pty == 2 || pty == 6)
                {
                    region_weather[regionNum] = 2;
                }
                else if( pty == 3 || pty == 7)
                {
                    region_weather[regionNum] = 3;
                }

                Debug.Log("���� ���� : " + pty);
            }

        }

                
    }
}
