using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;


// 기상청 API를 JSON으로 받아와 LitJson을 이용하여 파싱하는 스크립트
public class WeatherGetAPI : MonoBehaviour
{
    private string apiKey = "6c3v2vDyMl1%2BDCT98Ui1wMalyBfWpEdxTlC2grEFi4OTZWfB82Xkg1zQvlS37wyNaXxw4T4y1Ap2p7klKbJtdg%3D%3D";

    private string forecastUrl_srtNcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // 초단기 실황 - getUltraSrtNcst
    private string forecastUrl_VilageFcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // 단기 예보 - getVilageFcst

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
        // 기본값 지정
        string apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_data}&base_time={base_time}&nx={nx}&ny={ny}";

        if (UniteData.forecastTypeNum == 0)  // 초단기 실황
        {
            apiUrl = $"{forecastUrl_srtNcst}?serviceKey={apiKey}&dataType={dataType}&base_date={base_data}&base_time={base_time}&nx={nx}&ny={ny}";
        }
        else if(UniteData.forecastTypeNum == 1) // 단기 예보
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
}
