using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFace : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Texture head_base;
    [SerializeField] private Texture head_base_sad;

    [SerializeField] private Material head;

    void Start()
    {
        ChangeFace();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeFace()
    {
        // ºñ°¡ ¿À¸é(1, 4, 5)
        // ºñ¶û ´« °°ÀÌ ¿À´Â°Í(2, 6)µµ ¿ì¼±
        if(UniteData.pty == 1 || UniteData.pty == 2 || (UniteData.pty >= 4 && UniteData.pty <= 6))
        {
            // ¿ì´Â ¾ó±¼
            head.mainTexture = head_base_sad;
        }

        // ¸¼°Å³ª ´«ÀÌ ¿À¸é
        else
        {
            head.mainTexture = head_base;
        }
    }
}
