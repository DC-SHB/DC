using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;


// ���û API�� JSON���� �޾ƿ� LitJson�� �̿��Ͽ� �Ľ��ϴ� ��ũ��Ʈ
public class WeatherGetAPI : MonoBehaviour
{
    private string apiKey = "6c3v2vDyMl1%2BDCT98Ui1wMalyBfWpEdxTlC2grEFi4OTZWfB82Xkg1zQvlS37wyNaXxw4T4y1Ap2p7klKbJtdg%3D%3D";

    private string forecastUrl_srtNcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // �ʴܱ� ��Ȳ - getUltraSrtNcst
    private string forecastUrl_VilageFcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // �ܱ� ���� - getVilageFcst

    private string dataType = "JSON";
    string jsonResult;

    private int base_data = 20230608;
    private int base_time = 1400;
    private int nx = 60;
    private int ny = 127;
    

    void Awake()
    {
        StartCoroutine(GetWeatherData());
    }

    IEnumerator GetWeatherData()
    {
        // �⺻�� ����
        string apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_data}&base_time={base_time}&nx={nx}&ny={ny}";

        if (UniteData.forecastTypeNum == 0)  // �ʴܱ� ��Ȳ
        {
            apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_data}&base_time={base_time}&nx={nx}&ny={ny}";
        }
        else if(UniteData.forecastTypeNum == 1) // �ܱ� ����
        {
            apiUrl = $"{forecastUrl_VilageFcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_data}&base_time={base_time}&nx={nx}&ny={ny}";
        }
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                jsonResult = webRequest.downloadHandler.text;
                if (UniteData.forecastTypeNum == 0) InitDataGetUltraSrtNcst(jsonResult);
                else if (UniteData.forecastTypeNum == 1) InitDataGetVilageFcst(jsonResult);
                //Debug.Log(jsonResult);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
    }

    // �ʴܱ� ��Ȳ�� ����� ���
    public void InitDataGetUltraSrtNcst(string jsonResult)
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);
        JsonData items = ItemData["response"]["body"]["items"]["item"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];

            if ((string)item["category"] == "T1H") // ���(T1H)
            {
                UniteData.temp = float.Parse((string)item["obsrValue"]);
                Debug.Log("��� : " + UniteData.temp);
            }

            if((string)item["category"] == "PTY") // ���� ����(PTY) : ����(0) / ��(1) / ��&��(2) / ��(3) / �����(5) / ����ﴫ����(6) / ������(7)
            {
                UniteData.pty = int.Parse((string)item["obsrValue"]);
                Debug.Log("���� ���� : " + UniteData.pty);
            }

            if((string)item["category"] == "REH") // ����(REH) : %
            {
                UniteData.reh = int.Parse((string)item["obsrValue"]);
                Debug.Log("���� : " + UniteData.reh);
 
            }

            if((string)item["category"] == "WSD") // ǳ��(WSD) : �ٶ��� ���ϴ�(~3) / �ణ ��(4~8) / ��(9~13) / �ſ� ��(14~)
            {
                UniteData.wsd = float.Parse((string)item["obsrValue"]);
                Debug.Log("ǳ�� : " + UniteData.wsd);
            }
        }

    }

    // �ܱ� ������ ����� ���
    public void InitDataGetVilageFcst(string jsonResult)
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);
        JsonData items = ItemData["response"]["body"]["items"]["item"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];

            if ((string)item["category"] == "POP") // ����Ȯ��(POP) %
            {
                UniteData.pop = int.Parse((string)item["fcstValue"]);
                Debug.Log("����Ȯ�� : " + UniteData.pop);
            }

            if ((string)item["category"] == "PTY") // ��������(PTY) : ����(0) / ��&��(2) / ��(3) / �ҳ���(4)
            {
                UniteData.pty = int.Parse((string)item["fcstValue"]);
                Debug.Log("���� ���� : " + UniteData.pty);
            }

            if ((string)item["category"] == "WSD") // ǳ��(WSD) : �ٶ��� ���ϴ�(~3) / �ణ ��(4~8) / ��(9~13) / �ſ� ��(14~)
            {
                UniteData.wsd = float.Parse((string)item["fcstValue"]);
                Debug.Log("ǳ�� : " + UniteData.wsd);
            }

            if ((string)item["category"] == "SKY") // �ϴû���(SKY) : ����(0~5) / ���� ����(6~8) / �帲(9~10)
            {
                UniteData.sky = int.Parse((string)item["fcstValue"]);
                Debug.Log("�ϴ� ���� : " + UniteData.sky);
            }

            if ((string)item["category"] == "TMN") // �� �������(TMN)
            {
                UniteData.tmn = float.Parse((string)item["fcstValue"]);
                Debug.Log("�� ������� : " + UniteData.tmn);
            }

            if ((string)item["category"] == "TMX") // �� �ְ���(TMX)
            {
                UniteData.tmx = float.Parse((string)item["fcstValue"]);
                Debug.Log("�� �ְ��� : " + UniteData.tmx);
            }

        }
    
    }
}
