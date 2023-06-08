using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// ���� UI ���� ��ũ��Ʈ
public class QuizManager : MonoBehaviour
{
    [SerializeField] Text text_title;
    [SerializeField] Text text_queistion;
    [SerializeField] Text text_O;
    [SerializeField] Text text_X;

    private int QuizNum = 0;
    public int score = 0;
    public string date = "";

    // Start is called before the first frame update
    void Start()
    {
        QuizNum = 0;
        score = 0;
        date = "";

        text_title.text = "����Ǯ��";
        text_queistion.text = "���� ���� ������ Ǯ���?";
        text_O.text = "�������!";
        text_X.text = "����������!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartQuiz()
    {
        QuizNum = 1;
        score = 0;
        text_title.text = QuizNum.ToString() + "������";
        text_O.text = "�¾�!";
        text_X.text = "�ƴϾ�!";
    }

    public void SelectO()
    {
        if (QuizNum == 0 || QuizNum == 6)
        {
            StartQuiz();
        }
        else CheckAnswer(0);
    }

    public void SelectX()
    {
        if (QuizNum == 0 || QuizNum == 6)
        {
            SceneManager.LoadScene("QuizScore");
        }
        else CheckAnswer(1);
    }

    private void CheckAnswer(int answer)
    {
        // �����̸�
        /*if (answer)
        {
            score = score + 20;
        }*/
        NextQuestion();
    }

    private void NextQuestion()
    {
        QuizNum++;
        // ��� ���� ��
        if(QuizNum == 6)
        {
            text_title.text = "";
            text_queistion.text = "������!\n"+ score.ToString() + "���� �޾Ҿ�!";
            text_O.text = "�ٽ� �ҷ�!";
            text_X.text = "����������!";

            // ���� �ð� �� ���� ���
            date = DateTime.Now.ToString(("yyyy.MM.dd HH:mm"));
            Debug.Log(date);
        }

        else text_title.text = QuizNum.ToString() + "������";

    }
}
