using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System;
using Unity.VisualScripting;


// ���û API�� JSON���� �޾ƿ� LitJson�� �̿��Ͽ� �Ľ��ϴ� ��ũ��Ʈ
public class WeatherGetAPI : MonoBehaviour
{
    private string apiKey = "6c3v2vDyMl1%2BDCT98Ui1wMalyBfWpEdxTlC2grEFi4OTZWfB82Xkg1zQvlS37wyNaXxw4T4y1Ap2p7klKbJtdg%3D%3D";

    private string forecastUrl_srtNcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // �ʴܱ� ��Ȳ - getUltraSrtNcst
    private string forecastUrl_VilageFcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // �ܱ� ���� - getVilageFcst

    private string dataType = "JSON";
    string jsonResult;

    Dictionary<string, float> result;


    private int base_date;
    private int base_time;
    private int base_time_hh;
    private int base_time_mm;

    private float nx;
    private float ny;

    void Awake()
    {
        base_date = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
        base_time_hh = int.Parse(DateTime.Now.ToString("HH"));
        base_time_mm = int.Parse(DateTime.Now.ToString("mm"));

        if (base_time_mm <= 30)
        {// �ش� �ð� ��ǥ ������ �ڷᰡ ��� ���� �ð��� �������� 
            base_time_hh -= 1;
        }
        base_time = int.Parse(base_time_hh.ToString() + "00");

        result = dfs_xy_conf(UniteData.latitude, UniteData.longitude);
        nx = result["x"];
        ny = result["y"];

        Debug.Log("x : " + nx + " y : " + ny);
        StartCoroutine(GetWeatherData());
    }

    IEnumerator GetWeatherData()
    {
        // �⺻�� ����
        Debug.Log("data : " + base_date + "time " + base_time);
        string apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_date}&base_time={base_time}&nx={nx}&ny={ny}";

        if (UniteData.forecastTypeNum == 0)  // �ʴܱ� ��Ȳ
        {
            apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_date}&base_time={base_time}&nx={nx}&ny={ny}";
        }
        else if(UniteData.forecastTypeNum == 1) // �ܱ� ����
        {
            apiUrl = $"{forecastUrl_VilageFcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_date}&base_time={base_time}&nx={nx}&ny={ny}";
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
        Debug.Log("���� API ���� �Ϸ�");
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

    // ���� �浵 ��ǥ ��ȯ
    float RE = 6371.00877f; // ���� �ݰ�(km)
    float GRID = 5.0f; // ���� ����(km)
    float SLAT1 = 30.0f; // ���� ����1(degree)
    float SLAT2 = 60.0f; // ���� ����2(degree)
    float OLON = 126.0f; // ������ �浵(degree)
    float OLAT = 38.0f; // ������ ����(degree)
    float XO = 43f; // ������ X��ǥ(GRID)
    float YO = 136f; // ��1���� Y��ǥ(GRID)

    Dictionary<string, float> dfs_xy_conf(float v1, float v2)
    {
        float DEGRAD = Mathf.PI / 180.0f;
        float RADDEG = 180.0f / Mathf.PI;

        float re = RE / GRID;
        float slat1 = SLAT1 * DEGRAD;
        float slat2 = SLAT2 * DEGRAD;
        float olon = OLON * DEGRAD;
        float olat = OLAT * DEGRAD;

        float sn = Mathf.Tan((Mathf.PI * 0.25f + slat2 * 0.5f)) / Mathf.Tan(Mathf.PI * 0.25f + slat1 * 0.5f);
        sn = Mathf.Log(Mathf.Cos(slat1) / Mathf.Cos(slat2)) / Mathf.Log(sn);
        float sf = Mathf.Tan(Mathf.PI * 0.25f + slat1 * 0.5f);
        sf = Mathf.Pow(sf, sn) * Mathf.Cos(slat1) / sn;
        float ro = Mathf.Tan(Mathf.PI * 0.25f + olat * 0.5f);
        ro = re * sf / Mathf.Pow(ro, sn);

        Dictionary<string, float> rs = new Dictionary<string, float>();
        float ra, theta;

        rs["lat"] = v1;
        rs["lng"] = v2;
        ra = Mathf.Tan(Mathf.PI * 0.25f + (v1) * DEGRAD * 0.5f);
        ra = re * sf / Mathf.Pow(ra, sn);
        theta = v2 * DEGRAD - olon;
        if (theta > Mathf.PI) theta -= 2.0f * Mathf.PI;
        if (theta < -Mathf.PI) theta += 2.0f * Mathf.PI;
        theta *= sn;
        rs["x"] = Mathf.Floor(ra * Mathf.Sin(theta) + XO + 0.5f);
        rs["y"] = Mathf.Floor(ro - ra * Mathf.Cos(theta) + YO + 0.5f);
        return rs;
    }
}
