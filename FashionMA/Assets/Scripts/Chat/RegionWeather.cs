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
        // ����(60, 127), ��õ(73, 134), ����(92, 131), ��õ(55, 124), ����(60, 121),
        // ����(67, 100), û��(69, 106), �ȵ�(91, 106), ����(63, 89), â��(99, 77),
        // �뱸(89, 90), ���(102, 84), �λ�(98, 76), ����(58, 74), ����(50, 67), ����(52, 38), �︪(127, 127)


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
                InitDataGetUltraSrtNcst(jsonResult, regionNum);
                //Debug.Log(jsonResult);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
        Debug.Log("���� API ���� �Ϸ�");
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

            if ((string)item["category"] == "T1H") // ���(T1H)
            {
                temp = float.Parse((string)item["obsrValue"]);
                text_temp[regionNum].GetComponent<Text>().text= temp.ToString();
                Debug.Log("��� : " + temp);
            }

            // ����, ��, ��&��, ��
            if ((string)item["category"] == "PTY") // ���� ����(PTY) : ����(0) / ��(1) / ��&��(2) / ��(3) / �����(5) / ����ﴫ����(6) / ������(7)
            {
                pty = int.Parse((string)item["obsrValue"]);

                if (pty == 0) // �������� 0
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

                Debug.Log("���� ���� : " + pty);
            }

        }

    }
}
