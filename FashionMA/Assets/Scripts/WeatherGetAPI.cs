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

    // �ʴܱ� ��Ȳ ����
    public float temp = 0f; // �µ�
    public int pty = 0; // ���� ����
    public int reh = 0; // ����
    public float wsd = 0; // ǳ��

    // �ܱ� ���� ����
    public int pop = 0; // ���� Ȯ��
    public int sky = 0; // �ϴ� ����
    public float tmn = 0f; // �� �������
    public float tmx = 0f; // �� �ְ���

    public int forecastTypeNum = 1; // ������ ���� - �ʴܱ� ��Ȳ(0) , �ܱ� ����(1)

    void Start()
    {
        StartCoroutine(GetWeatherData());
    }

    IEnumerator GetWeatherData()
    {
        // �⺻�� ����
        string apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date=20230605&base_time=1400&nx=60&ny=127";

        if (forecastTypeNum == 0)  // �ʴܱ� ��Ȳ
        {
            apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date=20230605&base_time=1400&nx=60&ny=127";
        }
        else if(forecastTypeNum == 1) // �ܱ� ����
        {
            apiUrl = $"{forecastUrl_VilageFcst}?serviceKey={apiKey}&dataType={dataType}&base_date=20230605&base_time=1400&nx=60&ny=127";
        }
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                jsonResult = webRequest.downloadHandler.text;
                if (forecastTypeNum == 0) InitDataGetUltraSrtNcst(jsonResult);
                else if (forecastTypeNum == 1) InitDataGetVilageFcst(jsonResult);
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
                temp = float.Parse((string)item["obsrValue"]);
                Debug.Log("��� : " + temp);
            }

            if((string)item["category"] == "PTY") // ���� ����(PTY) : ����(0) / ��(1) / ��&��(2) / ��(3) / �����(5) / ����ﴫ����(6) / ������(7)
            {
                pty = int.Parse((string)item["obsrValue"]);
                Debug.Log("���� ���� : " + pty);
            }

            if((string)item["category"] == "REH") // ����(REH) : %
            {
                reh = int.Parse((string)item["obsrValue"]);
                Debug.Log("���� : " + reh);
 
            }

            if((string)item["category"] == "WSD") // ǳ��(WSD) : �ٶ��� ���ϴ�(~3) / �ణ ��(4~8) / ��(9~13) / �ſ� ��(14~)
            {
                wsd = float.Parse((string)item["obsrValue"]);
                Debug.Log("ǳ�� : " + wsd);
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
                pop = int.Parse((string)item["fcstValue"]);
                Debug.Log("����Ȯ�� : " + pop);
            }

            if ((string)item["category"] == "PTY") // ��������(PTY) : ����(0) / ��&��(2) / ��(3) / �ҳ���(4)
            {
                pty = int.Parse((string)item["fcstValue"]);
                Debug.Log("���� ���� : " + pty);
            }

            if ((string)item["category"] == "WSD") // ǳ��(WSD) : �ٶ��� ���ϴ�(~3) / �ణ ��(4~8) / ��(9~13) / �ſ� ��(14~)
            {
                wsd = float.Parse((string)item["fcstValue"]);
                Debug.Log("ǳ�� : " + wsd);
            }

            if ((string)item["category"] == "SKY") // �ϴû���(SKY) : ����(0~5) / ���� ����(6~8) / �帲(9~10)
            {
                sky = int.Parse((string)item["fcstValue"]);
                Debug.Log("�ϴ� ���� : " + sky);
            }

            if ((string)item["category"] == "TMN") // �� �������(TMN)
            {
                tmn = float.Parse((string)item["fcstValue"]);
                Debug.Log("�� ������� : " + tmn);
            }

            if ((string)item["category"] == "TMX") // �� �ְ���(TMX)
            {
                tmx = float.Parse((string)item["fcstValue"]);
                Debug.Log("�� �ְ��� : " + tmx);
            }

        }
    
    }
}
