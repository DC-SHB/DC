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
        // �� ����(1, 4, 5)
        // ��� �� ���� ���°�(2, 6)�� �켱
        if(UniteData.pty == 1 || UniteData.pty == 2 || (UniteData.pty >= 4 && UniteData.pty <= 6))
        {
            // ��� ��
            head.mainTexture = head_base_sad;
        }

        // ���ų� ���� ����
        else
        {
            head.mainTexture = head_base;
        }
    }
}
