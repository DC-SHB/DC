using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using UnityEngine;

public class WeatherAPI : MonoBehaviour
{
    private string url;
    private int base_date = 230605; // ���� ��¥
    private int base_time = 0425;// ��û �ð�
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetWeatherData()
    {
        Debug.Log(DateTime.Now.ToString("yyyyMMdd"));
        Console.WriteLine(DateTime.Now.ToString("HHmm"));




        url = "http://apis.data.go.kr/1360000/VilageFcstInfoService_2.0/";
        url += "getUltraSrtNcst";
        url += "?ServiceKey=" + "6c3v2vDyMl1%2BDCT98Ui1wMalyBfWpEdxTlC2grEFi4OTZWfB82Xkg1zQvlS37wyNaXxw4T4y1Ap2p7klKbJtdg%3D%3D";
        url += "&numOfRows=10";             // �������� ��� ��(Default : 10)
        url += "&pageNo=1";                 // ������ ��ȣ(Default : 1)
        url += "&dataType=JSON";             // ���� �ڷ�����(XML, JSON)
        url += "&base_date=" + base_date;   // ��û ��¥(yyMMdd)
        url += "&base_time=" + base_time;   // ��û �ð�(HHmm)
        url += "&nx=55";                    // ��û ���� x��ǥ
        url += "&ny=127";                   // ��û ���� y��ǥ

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        string results = string.Empty;
        HttpWebResponse response;
        using (response = request.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            results = reader.ReadToEnd();
        }

        // �޾ƿ� ��� xml �������� �Ľ�
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(results);
        XmlNodeList xmResponse = xml.GetElementsByTagName("response");
        XmlNodeList xmlList = xml.GetElementsByTagName("item");
    }
}
