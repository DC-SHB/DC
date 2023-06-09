using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using System;
using Unity.VisualScripting;


// 기상청 API를 JSON으로 받아와 LitJson을 이용하여 파싱하는 스크립트
public class WeatherGetAPI : MonoBehaviour
{
    private string apiKey = "6c3v2vDyMl1%2BDCT98Ui1wMalyBfWpEdxTlC2grEFi4OTZWfB82Xkg1zQvlS37wyNaXxw4T4y1Ap2p7klKbJtdg%3D%3D";

    private string forecastUrl_srtNcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // 초단기 실황 - getUltraSrtNcst
    private string forecastUrl_VilageFcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // 단기 예보 - getVilageFcst

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
        {// 해당 시각 발표 전에는 자료가 없어서 이전 시각을 기준으로 
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
        // 기본값 지정
        Debug.Log("data : " + base_date + "time " + base_time);
        string apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_date}&base_time={base_time}&nx={nx}&ny={ny}";

        if (UniteData.forecastTypeNum == 0)  // 초단기 실황
        {
            apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_date}&base_time={base_time}&nx={nx}&ny={ny}";
        }
        else if(UniteData.forecastTypeNum == 1) // 단기 예보
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
        Debug.Log("날씨 API 설정 완료");
    }

    // 초단기 실황을 출력할 경우
    public void InitDataGetUltraSrtNcst(string jsonResult)
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);
        JsonData items = ItemData["response"]["body"]["items"]["item"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];

            if ((string)item["category"] == "T1H") // 기온(T1H)
            {
                UniteData.temp = float.Parse((string)item["obsrValue"]);
                Debug.Log("기온 : " + UniteData.temp);
            }

            if((string)item["category"] == "PTY") // 강수 형태(PTY) : 없음(0) / 비(1) / 비&눈(2) / 눈(3) / 빗방울(5) / 빗방울눈날림(6) / 눈날림(7)
            {
                UniteData.pty = int.Parse((string)item["obsrValue"]);
                Debug.Log("강수 형태 : " + UniteData.pty);
            }

            if((string)item["category"] == "REH") // 습도(REH) : %
            {
                UniteData.reh = int.Parse((string)item["obsrValue"]);
                Debug.Log("습도 : " + UniteData.reh);
 
            }

            if((string)item["category"] == "WSD") // 풍속(WSD) : 바람이 약하다(~3) / 약간 강(4~8) / 강(9~13) / 매우 강(14~)
            {
                UniteData.wsd = float.Parse((string)item["obsrValue"]);
                Debug.Log("풍속 : " + UniteData.wsd);
            }
        }

    }

    // 단기 예보를 출력할 경우
    public void InitDataGetVilageFcst(string jsonResult)
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);
        JsonData items = ItemData["response"]["body"]["items"]["item"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];

            if ((string)item["category"] == "POP") // 강수확률(POP) %
            {
                UniteData.pop = int.Parse((string)item["fcstValue"]);
                Debug.Log("강수확률 : " + UniteData.pop);
            }

            if ((string)item["category"] == "PTY") // 강수형태(PTY) : 없음(0) / 비&눈(2) / 눈(3) / 소나기(4)
            {
                UniteData.pty = int.Parse((string)item["fcstValue"]);
                Debug.Log("강수 형태 : " + UniteData.pty);
            }

            if ((string)item["category"] == "WSD") // 풍속(WSD) : 바람이 약하다(~3) / 약간 강(4~8) / 강(9~13) / 매우 강(14~)
            {
                UniteData.wsd = float.Parse((string)item["fcstValue"]);
                Debug.Log("풍속 : " + UniteData.wsd);
            }

            if ((string)item["category"] == "SKY") // 하늘상태(SKY) : 맑음(0~5) / 구름 많음(6~8) / 흐림(9~10)
            {
                UniteData.sky = int.Parse((string)item["fcstValue"]);
                Debug.Log("하늘 상태 : " + UniteData.sky);
            }

            if ((string)item["category"] == "TMN") // 일 최저기온(TMN)
            {
                UniteData.tmn = float.Parse((string)item["fcstValue"]);
                Debug.Log("일 최저기온 : " + UniteData.tmn);
            }

            if ((string)item["category"] == "TMX") // 일 최고기온(TMX)
            {
                UniteData.tmx = float.Parse((string)item["fcstValue"]);
                Debug.Log("일 최고기온 : " + UniteData.tmx);
            }

        }
    
    }

    // 위도 경도 좌표 변환
    float RE = 6371.00877f; // 지구 반경(km)
    float GRID = 5.0f; // 격자 간격(km)
    float SLAT1 = 30.0f; // 투영 위도1(degree)
    float SLAT2 = 60.0f; // 투영 위도2(degree)
    float OLON = 126.0f; // 기준점 경도(degree)
    float OLAT = 38.0f; // 기준점 위도(degree)
    float XO = 43f; // 기준점 X좌표(GRID)
    float YO = 136f; // 기1준점 Y좌표(GRID)

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
