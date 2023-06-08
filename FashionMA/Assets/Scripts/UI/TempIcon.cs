using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempIcon : MonoBehaviour
{
    [SerializeField] TMP_Text tempUI;

    // Start is called before the first frame update
    void Start()
    {
        ChangeTempUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ChangeTempUI()
    {
        tempUI.text = UniteData.temp.ToString();
        tempUI.text = tempUI.text + "им";
    }
}
