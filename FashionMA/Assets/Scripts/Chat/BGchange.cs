using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGchange : MonoBehaviour
{
    [SerializeField] private GameObject BG;
    private int hour;
    [SerializeField] private Sprite[] sprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hour = DateTime.Now.Hour;
        if (hour >= 0 && hour < 5)
        {
            BG.GetComponent<Image>().sprite = sprite[0];
        }
        if (hour >= 5 && hour < 9)
        {
            BG.GetComponent<Image>().sprite = sprite[1];
        }
        if (hour >= 9 && hour < 17)
        {
            BG.GetComponent<Image>().sprite = sprite[2];
        }
        if (hour >= 17 && hour < 21)
        {
            BG.GetComponent<Image>().sprite = sprite[3];
        }
        if (hour >= 21 && hour < 24)
        {
            BG.GetComponent<Image>().sprite = sprite[4];
        }
    }
}
