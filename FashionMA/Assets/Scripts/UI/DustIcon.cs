using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DustIcon : MonoBehaviour
{
    public Sprite[] dustIcon = new Sprite[4];
    public Image DustUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!UniteData.dust.Equals("0"))
        {
            string dust = UniteData.dust;
            if (dust.Equals("ÁÁÀ½")) DustUI.sprite = dustIcon[0];
            if (dust.Equals("º¸Åë")) DustUI.sprite = dustIcon[1];
            if (dust.Equals("³ª»Ý")) DustUI.sprite = dustIcon[2];
            if (dust.Equals("¸Å¿ì³ª»Ý")) DustUI.sprite = dustIcon[3];
        }
    }
}
