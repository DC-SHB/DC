using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteData : MonoBehaviour
{
    // 날씨 관련 데이터

    // 초단기 실황 변수
    public static float temp = 0f; // 온도
    public static int pty = 0; // 강수 형태
    public static int reh = 0; // 습도
    public static float wsd = 0; // 풍속

    // 단기 예보 변수
    public static int pop = 0; // 강수 확률
    public static int sky = 0; // 하늘 상태
    public static float tmn = 0f; // 일 최저기온
    public static float tmx = 0f; // 일 최고기온

}
