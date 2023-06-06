using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class LoadGPSInfo : MonoBehaviour
{
    public Text latitude_text;
    public Text longitude_text;

    private float latitude = 0;
    private float longitude = 0;
    private float waitTime = 0;
    private float maxWaitTime = 10.0f;
    private float resendTime = 1.0f;
    private bool receiveGPS = false;

    void Start()
    {
        StartCoroutine(GPS_On());
    }

    public IEnumerator GPS_On()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            while (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
            {
                yield return null;
            }
        }

        if (!Input.location.isEnabledByUser)
        {
            latitude_text.text = "GPS Off";
            longitude_text.text = "GPS Off";
            yield break;
        }

        Input.location.Start();

        while (Input.location.status == LocationServiceStatus.Initializing && waitTime < maxWaitTime)
        {
            yield return new WaitForSeconds(1.0f);
            waitTime++;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            latitude_text.text = "위치 정보 수신 실패";
            longitude_text.text = "위치 정보 수신 실패";
        }

        if (waitTime >= maxWaitTime)
        {
            latitude_text.text = "응답 대기 시간 초과";
            longitude_text.text = "응답 대기 시간 초과";
        }

        LocationInfo location = Input.location.lastData;
        receiveGPS = true;
        while (receiveGPS)
        {
            location = Input.location.lastData;
            latitude = location.latitude * 1.0f;
            longitude = location.longitude * 1.0f;

            latitude_text.text = "위도 : " + latitude.ToString();
            longitude_text.text = "경도 : " + longitude.ToString();

            yield return new WaitForSeconds(resendTime);
        }
    }
}