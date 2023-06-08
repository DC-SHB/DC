using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 퀴즈 화면에서 메인으로 가는 버튼 스크립트
public class GotoMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BtnGotoMain()
    {
        Debug.Log("123");
        SceneManager.LoadScene("Main");
    }
}
