using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClothes : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject man1, man2, man3;

    void Start()
    {
        man1.SetActive(false);
        man2.SetActive(false);
        man3.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (UniteData.CharacterLoad_clothes)
        {
            ChangeClothes();
            UniteData.CharacterLoad_clothes = false;
        }
    }

    void ChangeClothes()
    {
        if (UniteData.temp >= 23.0f) man1.SetActive(true);
        if (UniteData.temp > 17.0f && UniteData.temp < 23.0f) man2.SetActive(true);
        if (UniteData.temp < 17.0f) man3.SetActive(true);
    }
}
