using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteData : MonoBehaviour
{
    // ���� ���� ������

    // ������ ���� - �ʴܱ� ��Ȳ(0) , �ܱ� ����(1)
    public static int forecastTypeNum = 1;

    // �ʴܱ� ��Ȳ ���� 
    public static float temp = 0f;// ���(T1H)
    // ���� ����(PTY) : ����(0) / ��(1) / ��&��(2) / ��(3) / �ҳ���(4) / �����(5) / ����ﴫ����(6) / ������(7)
    public static int pty = 1; // (�ʴܱ� + �ܱ�)
    public static int reh = 0;  // ����(REH) : %
    // ǳ��(WSD) : �ٶ��� ���ϴ�(~3) / �ణ ��(4~8) / ��(9~13) / �ſ� ��(14~)
    public static float wsd = 0; // (�ʴܱ� + �ܱ�)

    // �ܱ� ���� ����
    public static int pop = 0; // ����Ȯ��(POP) %
    public static int sky = 0;  // �ϴû���(SKY) : ����(0~5) / ���� ����(6~8) / �帲(9~10)
    public static float tmn = 0f; // �� �������(TMN)
    public static float tmx = 0f; // �� �ְ���(TMX)

}
