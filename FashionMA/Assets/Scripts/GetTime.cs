using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTime : MonoBehaviour
{
    private string time;
    private int hour;
    public Text timeText;
    public Light light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time = DateTime.Now.ToString(("MM¿ù ddÀÏ HH:mm"));
        timeText.text = time;
        hour = DateTime.Now.Hour;
        if (hour >= 0 && hour < 5)
        {
            light.intensity = 0.9f;
            light.color = Color.white / 1.77f;
        }
        if (hour >= 5 && hour < 9)
        {
            light.intensity = 0.65f;
            light.color = Color.white;
        }
        if (hour >= 9 && hour < 17)
        {
            light.intensity = 0.8f;
            light.color = Color.white;
        }
        if (hour >= 17 && hour < 21)
        {
            light.intensity = 0.77f;
            light.color = Color.white / 1.77f;
        }
        if (hour >= 21 && hour < 24)
        {
            light.intensity = 0.65f;
            light.color = Color.white / 1.77f;
        }
    }
}
