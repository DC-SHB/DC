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
    private string dataType = "JSON";
    string jsonResult, jsonTodayResult;

    Dictionary<string, float> result;

    private int base_time;

    private float nx;
    private float ny;

    private bool startAPI;


    void Awake()
    {
        UniteData.UILoad_weatherIcon = false;
        UniteData.UILoad_temp = false;
        UniteData.UILoad_wind = false;
        UniteData.UILoad_bgweather = false;
        UniteData.CharacterLoad_clothes = false;
        UniteData.CharacterLoad_face = false;

        // 현재 시간 구하기
        DateTime currentTime = DateTime.Now;

        int baseHour = currentTime.Hour;

        if (currentTime.Minute < 40)
        {
            if(currentTime.Hour == 0) baseHour = 23;
            else baseHour = currentTime.Hour - 1;
        }

        // 기상청 API에 사용할 base_date와 base_time 설정
        UniteData.base_date = int.Parse(currentTime.ToString("yyyyMMdd"));
        base_time = baseHour * 100;
        if(base_time < 1000)
        {
            UniteData.base_time_s = "0" + base_time.ToString();
        }
        else
        {
            UniteData.base_time_s = base_time.ToString();
        }

        result = dfs_xy_conf(UniteData.latitude, UniteData.longitude);
        nx = result["x"];
        ny = result["y"];

        Debug.Log("x : " + nx + " y : " + ny);
    }

    void Update()
    {
        if(UniteData.latitude != 0 && UniteData.longitude != 0 && !startAPI)
        {
            StartCoroutine(GetWeatherData());
            StartCoroutine(GetTodayWeatherData());
            startAPI = true;
            Debug.Log("chk");
        }
    }

    IEnumerator GetWeatherData()
    {
        // 기본값 지정
        Debug.Log("data : " + UniteData.base_date + " time " + UniteData.base_time_s);
        string apiUrl = $"{UniteData.forecastUrl_srtNcst}?serviceKey={UniteData.apiKey}&dataType={dataType}&base_date={UniteData.base_date}&base_time={UniteData.base_time_s}&nx={nx}&ny={ny}";
        //Debug.Log(apiUrl);
        if (UniteData.forecastTypeNum == 0)  // 초단기 실황
        {
            apiUrl = $"{UniteData.forecastUrl_srtNcst}?serviceKey={UniteData.apiKey}&dataType={dataType}&base_date={UniteData.base_date}&base_time={UniteData.base_time_s}&nx={nx}&ny={ny}";
        }
        else if(UniteData.forecastTypeNum == 1) // 단기 예보
        {
            //MakeBaseTime();
            apiUrl = $"{UniteData.forecastUrl_VilageFcst}?serviceKey={UniteData.apiKey}&dataType={dataType}&numOfRows=290&pageNo=1&base_date={UniteData.base_date}&base_time=0200&nx={nx}&ny={ny}";
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

    IEnumerator GetTodayWeatherData()
    {
        string apiUrl = $"{UniteData.forecastUrl_VilageFcst}?serviceKey={UniteData.apiKey}&dataType={dataType}&numOfRows=290&pageNo=1&base_date={UniteData.base_date}&base_time=0200&nx={nx}&ny={ny}";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                jsonTodayResult = webRequest.downloadHandler.text;
                InitDataGetVilageFcst(jsonTodayResult);
                //Debug.Log(jsonResult);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
        Debug.Log("단기 예보 API 설정 완료");
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
                UniteData.UILoad_temp = true;
                UniteData.CharacterLoad_clothes = true;
                Debug.Log("기온 : " + UniteData.temp);
            }

            if((string)item["category"] == "PTY") // 강수 형태(PTY) : 없음(0) / 비(1) / 비&눈(2) / 눈(3) / 빗방울(5) / 빗방울눈날림(6) / 눈날림(7)
            {
                UniteData.pty = int.Parse((string)item["obsrValue"]);
                UniteData.UILoad_weatherIcon = true;
                UniteData.UILoad_bgweather = true;
                UniteData.CharacterLoad_face = true;
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
                UniteData.UILoad_wind = true;
                Debug.Log("풍속 : " + UniteData.wsd);
            }
        }
    }

    //public void MakeBaseTime()
    //{
    //    int time = DateTime.Now.Hour * 100 + DateTime.Now.Minute;
    //    if (time > 210) UniteData.base_time2 = "0200";
    //    if (time > 510) UniteData.base_time2 = "0500";
    //    if (time > 810) UniteData.base_time2 = "0800";
    //    if (time > 1110) UniteData.base_time2 = "1100";
    //    if (time > 1410) UniteData.base_time2 = "1400";
    //    if (time > 1710) UniteData.base_time2 = "1700";
    //    if (time > 2010) UniteData.base_time2 = "2000";
    //    if (time > 2310 || time <= 210) UniteData.base_time2 = "2300";
    //    Debug.Log(UniteData.base_time2);
    //}

    // 위도 경도 좌표 변환

    // 단기 예보를 출력할 경우
    public void InitDataGetVilageFcst(string jsonResult)
    {
        JsonData ItemData = JsonMapper.ToObject(jsonResult);
        JsonData items = ItemData["response"]["body"]["items"]["item"];

        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            int fsctTime = int.Parse((string)item["fcstTime"]);
            fsctTime /= 100;

            if ((string)item["category"] == "POP") // 강수확률(POP) %
            {
                UniteData.todayWeather[fsctTime, 0] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "PTY") // 강수형태(PTY) : 없음(0) / 비(1) / 비&눈(2) / 눈(3) / 소나기(4)
            {
                UniteData.todayWeather[fsctTime, 1] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "PCP")
            {
                UniteData.todayWeather[fsctTime, 2] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "REH")
            {
                UniteData.todayWeather[fsctTime, 3] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "SNO")
            {
                UniteData.todayWeather[fsctTime, 4] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "SKY") // 하늘상태(SKY) : 맑음(0~5) / 구름 많음(6~8) / 흐림(9~10)
            {
                UniteData.todayWeather[fsctTime, 5] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "TMP")
            {
                UniteData.todayWeather[fsctTime, 6] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "UUU")
            {
                UniteData.todayWeather[fsctTime, 7] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "VVV")
            {
                UniteData.todayWeather[fsctTime, 8] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "WAV")
            {
                UniteData.todayWeather[fsctTime, 9] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "VEC")
            {
                UniteData.todayWeather[fsctTime, 10] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "WSD") // 풍속(WSD) : 바람이 약하다(~3) / 약간 강(4~8) / 강(9~13) / 매우 강(14~)
            {
                UniteData.todayWeather[fsctTime, 11] = (string)item["fcstValue"];
            }

            if ((string)item["category"] == "TMN") // 일 최저기온(TMN)
            {
                UniteData.tmn = float.Parse((string)item["fcstValue"]);
            }
            
            if ((string)item["category"] == "TMX") // 일 최고기온(TMX)
            {
                UniteData.tmx = float.Parse((string)item["fcstValue"]);
            }
        }

        //for (int i=0; i< UniteData.todayWeather.GetLength(0); i++)
        //{
        //    for (int j = 0; j < UniteData.todayWeather.GetLength(1); j++)
        //    {
        //        if (UniteData.todayWeather[i, j] != null) Debug.Log("[" + i + ", " + j + "] " + UniteData.todayWeather[i, j]);
        //    }
        //}
    }

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
