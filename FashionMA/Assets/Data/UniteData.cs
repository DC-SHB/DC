using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteData : MonoBehaviour
{
    // 날씨 관련 데이터

    // 선택할 예보 - 초단기 실황(0) , 단기 예보(1)
    public static int forecastTypeNum = 1;

    // 초단기 실황 변수 
    public static float temp = 0f;// 기온(T1H)
    // 강수 형태(PTY) : 없음(0) / 비(1) / 비&눈(2) / 눈(3) / 소나기(4) / 빗방울(5) / 빗방울눈날림(6) / 눈날림(7)
    public static int pty = 1; // (초단기 + 단기)
    public static int reh = 0;  // 습도(REH) : %
    // 풍속(WSD) : 바람이 약하다(~3) / 약간 강(4~8) / 강(9~13) / 매우 강(14~)
    public static float wsd = 0; // (초단기 + 단기)

    // 단기 예보 변수
    public static int pop = 0; // 강수확률(POP) %
    public static int sky = 0;  // 하늘상태(SKY) : 맑음(0~5) / 구름 많음(6~8) / 흐림(9~10)
    public static float tmn = 0f; // 일 최저기온(TMN)
    public static float tmx = 0f; // 일 최고기온(TMX)

}
