using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 날씨에 따라 캐릭터 표정 변화 스크립트
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
        // 비가 오면(1, 4, 5)
        // 비랑 눈 같이 오는것(2, 6)도 우선
        if(UniteData.pty == 1 || UniteData.pty == 2 || (UniteData.pty >= 4 && UniteData.pty <= 6))
        {
            // 우는 얼굴
            head.mainTexture = head_base_sad;
        }

        // 맑거나 눈이 오면
        else
        {
            head.mainTexture = head_base;
        }
    }
}
