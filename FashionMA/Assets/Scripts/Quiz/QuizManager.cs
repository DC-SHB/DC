using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 퀴즈 UI 관리 스크립트
public class QuizManager : MonoBehaviour
{
    [SerializeField] Text text_title;
    [SerializeField] Text text_queistion;
    [SerializeField] Button btn_O;
    [SerializeField] Button btn_X;
    [SerializeField] Text text_O;
    [SerializeField] Text text_X;
    [SerializeField] Text text_check;

    private int QuizNum = 0;
    public int score = 0;
    public string date = "";

    // Start is called before the first frame update
    void Start()
    {
        QuizNum = 0;
        score = 0;
        date = "";

        text_check.text = "";
        text_title.text = "퀴즈풀기";
        text_queistion.text = "날씨 관련 문제를 풀어볼래?";
        text_O.text = "퀴즈시작!";
        text_X.text = "점수판으로!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartQuiz()
    {
        QuizNum = 1;
        score = 0;
        text_title.text = QuizNum.ToString() + "번문제";
        text_O.text = "맞아!";
        text_X.text = "아니야!";
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
        btn_O.enabled = false;
        btn_X.enabled = false;
        // 정답이면
        /*if (answer)
        {
        text_check.text = "O";
           StartCoroutine(ShowCheckAnswer());
           score = score + 20;
         }*/

        // 오답이면
        /*
        else { 
        text_check.text = "X";
            StartCoroutine(ShowCheckAnswer());
        }
         */
        NextQuestion();
    }

    private void NextQuestion()
    {
        QuizNum++;

        btn_O.enabled = true;
        btn_X.enabled = true;

        // 모든 문제 끝
        if (QuizNum == 6)
        {
            text_title.text = "";
            text_queistion.text = "축하해!\n"+ score.ToString() + "점을 받았어!";
            text_O.text = "다시 할래!";
            text_X.text = "점수판으로!";

            // 현재 시간 및 점수 기록
            date = DateTime.Now.ToString(("yyyy.MM.dd HH:mm"));
            Debug.Log(date);
        }

        else text_title.text = QuizNum.ToString() + "번문제";
    }

    private IEnumerator ShowCheckAnswer()
    {
        text_check.color = new Color(text_check.color.r, text_check.color.g, text_check.color.b, 1);
        while (text_check.color.a > 0.0f)
        {
            text_check.color = new Color(text_check.color.r, text_check.color.g, text_check.color.b, text_check.color.a - (Time.deltaTime / 2.0f));
            yield return null;
        }
    }
}
