using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ���� ȭ�鿡�� �������� ���� ��ư ��ũ��Ʈ
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
