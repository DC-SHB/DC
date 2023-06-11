using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteData : MonoBehaviour
{
    // ���� ���� ������
    public static int base_date;
    public static string base_time_s;
    
    public static string apiKey = "6c3v2vDyMl1%2BDCT98Ui1wMalyBfWpEdxTlC2grEFi4OTZWfB82Xkg1zQvlS37wyNaXxw4T4y1Ap2p7klKbJtdg%3D%3D";
    public static string forecastUrl_srtNcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getUltraSrtNcst"; // �ʴܱ� ��Ȳ - getUltraSrtNcst
    public static string forecastUrl_VilageFcst = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/getVilageFcst"; // �ܱ� ���� - getVilageFcst


    // ������ ���� - �ʴܱ� ��Ȳ(0) , �ܱ� ����(1)
    public static int forecastTypeNum = 1;

    // �ʴܱ� ��Ȳ ���� 
    public static float temp = 0f;// ���(T1H)
    // ���� ����(PTY) : ����(0) / ��(1) / ��&��(2) / ��(3) / �ҳ���(4) / �����(5) / ����ﴫ����(6) / ������(7)
    public static int pty = 0; // (�ʴܱ� + �ܱ�)
    public static int reh = 0;  // ����(REH) : %
    // ǳ��(WSD) : �ٶ��� ���ϴ�(~3) / �ణ ��(4~8) / ��(9~13) / �ſ� ��(14~)
    public static float wsd = 0; // (�ʴܱ� + �ܱ�)

    // �ܱ� ���� ����
    public static int pop = 0; // ����Ȯ��(POP) %
    public static int sky = 0;  // �ϴû���(SKY) : ����(0~5) / ���� ����(6~8) / �帲(9~10)
    public static float tmn = 0f; // �� �������(TMN)
    public static float tmx = 0f; // �� �ְ���(TMX)

    public static string[, ] todayWeather = new string[24, 14]; //0 : �ð�, 1 : POP, PTY, PCP, REH, SNO, SKY, TMP, UUU, VVV, WAV, VEC, WSD

    //����, �浵
    public static float latitude = 0f;
    public static float longitude = 0;

    //��ġ
    public static string location0 = "0";
    public static string location1 = "0";
    public static string location2 = "0";

    //�̼�����
    public static string dust = "0";


    // ���� ������ ����
    public static int maxEntries = 8; // �ִ� ��Ʈ�� ����
    public static string[] logDate = new string[maxEntries]; // ��¥ �迭
    public static int[] logScore = new int[maxEntries]; // ���� �迭

    public static void SaveUserData()
    {
        // �������� ������ ����
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
        Debug.Log("���� ������ �ʱ�ȭ");
    }

    public static void LoadLeaderboard()
    {
        // ���� ������ �ε�
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
