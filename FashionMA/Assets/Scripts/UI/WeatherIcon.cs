using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] weatherIcons; // ����, ��, ��&��, ��
    [SerializeField] private Image weatherUI; // ����ȭ�� ����UI

    // Start is called before the first frame update
    void Start()
    {
        ChangeWeatherUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeWeatherUI()
    {
        Debug.Log("pty : " + UniteData.pty);
        // ���� ����(PTY) : ����(0) / ��(1) / ��&��(2) / ��(3) / �ҳ���(4) / �����(5) / ����ﴫ����(6) / ������(7)
        if (UniteData.pty == 0)
        {
            weatherUI.sprite = weatherIcons[0];
        }

        else if (UniteData.pty == 1 || UniteData.pty == 4 || UniteData.pty == 5)
        {
            weatherUI.sprite = weatherIcons[1];
        }
            
        else if(UniteData.pty == 2 || UniteData.pty == 6)
        {
            weatherUI.sprite = weatherIcons[2];
        }
        else if(UniteData.pty == 3 || UniteData.pty == 7)
        {
            weatherUI.sprite = weatherIcons[3];
        }
    }
}
