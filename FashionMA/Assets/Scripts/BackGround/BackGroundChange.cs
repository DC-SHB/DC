using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 배경 관리 스크립트
public class BackGroundChange : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] particles;
    private ParticleSystem rain;
    private ParticleSystem snow;
    private ParticleSystem wind;

    // Start is called before the first frame update
    void Start()
    {
        // 초 단기 실황 호출
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
        // 강수 형태(PTY) : 없음(0) / 비(1) / 비 & 눈(2) / 눈(3) / 소나기(4) / 빗방울(5) / 빗방울눈날림(6) / 눈날림(7)
        // 풍속(WSD) : 바람이 약하다(~3) / 약간 강(4~8) / 강(9~13) / 매우 강(14~)

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
