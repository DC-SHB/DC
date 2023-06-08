using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� ���� ��ũ��Ʈ
public class BackGroundChange : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    private ParticleSystem rain;
    private ParticleSystem snow;
    private ParticleSystem wind;

    // Start is called before the first frame update
    void Start()
    {
        // �� �ܱ� ��Ȳ ȣ��
        UniteData.forecastTypeNum = 0;

        for(int i =0; i < particles.Length; i++)
        {
            particles[i].gameObject.SetActive(false);
        }

        rain = particles[0];
        snow = particles[1];
        wind = particles[2];

        ChangeBackGround_Weather();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeBackGround_Weather()
    {
        // ���� ����(PTY) : ����(0) / ��(1) / �� & ��(2) / ��(3) / �ҳ���(4) / �����(5) / ����ﴫ����(6) / ������(7)
        // ǳ��(WSD) : �ٶ��� ���ϴ�(~3) / �ణ ��(4~8) / ��(9~13) / �ſ� ��(14~)

        if (UniteData.pty == 0)
        {
            rain.gameObject.SetActive(false);
            snow.gameObject.SetActive(false);
        }

        // 1, 2, 4, 5, 6
        if(UniteData.pty == 1 || UniteData.pty == 2 || (UniteData.pty >=4 && UniteData.pty <= 6))
        {
            rain.gameObject.SetActive(true);
        }

        // 2, 3, 6, 7
        if(UniteData.pty == 2 || UniteData.pty == 3 || UniteData.pty == 6 || UniteData.pty == 7)
        {
            snow.gameObject.SetActive(true);
        }

        if(UniteData.wsd <= 8)
        {
            wind.gameObject.SetActive(false);
        }

        else if(UniteData.wsd >= 9 && UniteData.wsd <= 14)
        {
            wind.gameObject.SetActive(true);
        }

    }
}
