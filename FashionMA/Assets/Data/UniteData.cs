using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteData : MonoBehaviour
{
    // 날씨 관련 데이터
    public static int base_date;
    public static string base_time_s;
    
    public static string apiKey = "6c3v2vDyMl1%2BDCT98Ui1wMalyBfWpEdxTlC2grEFi4OTZWfB82Xkg1zQvlS37wyNaXxw4T4y1Ap2p7klKbJtdg%3D%3D";
    public static string forecastUrl_srtNcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // 초단기 실황 - getUltraSrtNcst
    public static string forecastUrl_VilageFcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // 단기 예보 - getVilageFcst


    // 선택할 예보 - 초단기 실황(0) , 단기 예보(1)
    public static int forecastTypeNum = 1;

    // 초단기 실황 변수 
    public static float temp = 0f;// 기온(T1H)
    // 강수 형태(PTY) : 없음(0) / 비(1) / 비&눈(2) / 눈(3) / 소나기(4) / 빗방울(5) / 빗방울눈날림(6) / 눈날림(7)
    public static int pty = 0; // (초단기 + 단기)
    public static int reh = 0;  // 습도(REH) : %
    // 풍속(WSD) : 바람이 약하다(~3) / 약간 강(4~8) / 강(9~13) / 매우 강(14~)
    public static float wsd = 0; // (초단기 + 단기)

    // 단기 예보 변수
    public static int pop = 0; // 강수확률(POP) %
    public static int sky = 0;  // 하늘상태(SKY) : 맑음(0~5) / 구름 많음(6~8) / 흐림(9~10)
    public static float tmn = 0f; // 일 최저기온(TMN)
    public static float tmx = 0f; // 일 최고기온(TMX)

    public static string[, ] todayWeather = new string[24, 14]; //0 : 시간, 1 : POP, PTY, PCP, REH, SNO, SKY, TMP, UUU, VVV, WAV, VEC, WSD

    //위도, 경도
    public static float latitude = 0f;
    public static float longitude = 0;

    //위치
    public static string location0 = "0";
    public static string location1 = "0";
    public static string location2 = "0";

    //미세먼지
    public static string dust = "0";


    // 유저 데이터 관련
    public static int maxEntries = 8; // 최대 엔트리 개수
    public static string[] logDate = new string[maxEntries]; // 날짜 배열
    public static int[] logScore = new int[maxEntries]; // 점수 배열

    public static void SaveUserData()
    {
        // 리더보드 데이터 저장
        string scoreString = string.Join(",", logScore);
        string dateString = string.Join(",", logDate);

        PlayerPrefs.SetString("LeaderboardScores", scoreString);
        PlayerPrefs.SetString("LeaderboardDates", dateString);

        PlayerPrefs.Save();
    }

    public static void ResetUserData()
    {
        maxEntries = 8;
        logDate = new string[maxEntries];
        logScore = new int[maxEntries];

        SaveUserData();
        Debug.Log("유저 데이터 초기화");
    }

    public static void LoadLeaderboard()
    {
        // 기존 데이터 로드
        string scoreString = PlayerPrefs.GetString("LeaderboardScores", string.Empty);
        string dateString = PlayerPrefs.GetString("LeaderboardDates", string.Empty);

        if (!string.IsNullOrEmpty(scoreString) && !string.IsNullOrEmpty(dateString))
        {
            string[] scoreArray = scoreString.Split(',');
            string[] dateArray = dateString.Split(',');

            if (scoreArray.Length == UniteData.maxEntries && dateArray.Length == UniteData.maxEntries)
            {
                for (int i = 0; i < UniteData.maxEntries; i++)
                {
                    UniteData.logScore[i] = int.Parse(scoreArray[i]);
                    UniteData.logDate[i] = dateArray[i];
                }
            }
        }
    }
}
