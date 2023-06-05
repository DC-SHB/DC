using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class WeatherGetAPI : MonoBehaviour
{
    private string apiKey = "6c3v2vDyMl1%2BDCT98Ui1wMalyBfWpEdxTlC2grEFi4OTZWfB82Xkg1zQvlS37wyNaXxw4T4y1Ap2p7klKbJtdg%3D%3D";
    // 초단기 실황 - getUltraSrtNcst
    // 초단기 예보 - getUltraSrtFcst
    // 단기 예보 - getVilageFcst
    // 예보 버전 - getFcstVersion
    private string forecastUrl = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst";
    private string dataType = "JSON";
    string jsonResult;

    void Start()
    {
        StartCoroutine(GetWeatherData());
    }

    IEnumerator GetWeatherData()
    {
        string apiUrl = $"{forecastUrl}?serviceKey={apiKey}&dataType={dataType}&base_date=20230605&base_time=1400&nx=60&ny=127";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                jsonResult = webRequest.downloadHandler.text;
                Debug.Log(jsonResult);
            }
            else
            {
                Debug.LogError($"Error: {webRequest.error}");
            }
        }
    }

    // 초단기 실황을 출력할 경우
    public void InitDataGetUltraSrtNcst()
    {
        // JsonData ItemData = JsonMapper.ToObject(jsonResult);
        // 기온(T1H)

        // 강수량(RN1)

        // 강수형태(PTY)

        // 습도(REH)

        // 풍속(WSD)


        // string temp = ItemData[""]
    }

    // 단기 예보를 출력할 경우
    public void InitDataGetVilageFcst()
    {

    }
}
