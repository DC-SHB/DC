using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class LoadGPSInfo : MonoBehaviour
{
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
            Debug.Log("GPS Off");
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
            Debug.Log("��ġ ���� ���� ����");
        }

        if (waitTime >= maxWaitTime)
        {
            Debug.Log("���� ��� �ð� �ʰ�");
        }

        LocationInfo location = Input.location.lastData;
        receiveGPS = true;
        while (receiveGPS)
        {
            location = Input.location.lastData;
            latitude = location.latitude * 1.0f;
            longitude = location.longitude * 1.0f;

            Debug.Log("���� : " + latitude.ToString());
            Debug.Log("�浵 : " + longitude.ToString());

            UniteData.latitude = latitude;
            UniteData.longitude = longitude;

            yield return new WaitForSeconds(resendTime);
        }
    }
}