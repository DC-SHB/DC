using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFace : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeFace()
    {
        // 비가 오면(1, 4, 5)
        // 비랑 눈 같이 오는것(2, 6)도 우선
        if(UniteData.pty == 1 || UniteData.pty == 2 || (UniteData.pty >= 4 && UniteData.pty <= 6))
        {
            
        }

        // 맑거나 눈이 오면
        else
        {

        }
    }
}
