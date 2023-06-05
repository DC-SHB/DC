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
    private int base_date = 230605; // 현재 날짜
    private int base_time = 0425;// 요청 시간
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
        url += "&numOfRows=10";             // 한페이지 결과 수(Default : 10)
        url += "&pageNo=1";                 // 페이지 번호(Default : 1)
        url += "&dataType=JSON";             // 받을 자료형식(XML, JSON)
        url += "&base_date=" + base_date;   // 요청 날짜(yyMMdd)
        url += "&base_time=" + base_time;   // 요청 시간(HHmm)
        url += "&nx=55";                    // 요청 지역 x좌표
        url += "&ny=127";                   // 요청 지역 y좌표

        var request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";
        string results = string.Empty;
        HttpWebResponse response;
        using (response = request.GetResponse() as HttpWebResponse)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            results = reader.ReadToEnd();
        }

        // 받아온 결과 xml 형식으로 파싱
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(results);
        XmlNodeList xmResponse = xml.GetElementsByTagName("response");
        XmlNodeList xmlList = xml.GetElementsByTagName("item");
    }
}
