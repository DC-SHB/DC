using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeatherIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] weatherIcons; // ¸¼À½, ºñ, ºñ&´«, ´«
    [SerializeField] private Image weatherUI; // ¸ÞÀÎÈ­¸é ³¯¾¾UI

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
        // °­¼ö ÇüÅÂ(PTY) : ¾øÀ½(0) / ºñ(1) / ºñ&´«(2) / ´«(3) / ¼Ò³ª±â(4) / ºø¹æ¿ï(5) / ºø¹æ¿ï´«³¯¸²(6) / ´«³¯¸²(7)
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
