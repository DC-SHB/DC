using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationIcon : MonoBehaviour
{
    public Text locationUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!UniteData.location0.Equals("0"))
        {
            string location = UniteData.location0 + " " + UniteData.location1 + " " + UniteData.location2;
            locationUI.text = location;
        }
    }
}
