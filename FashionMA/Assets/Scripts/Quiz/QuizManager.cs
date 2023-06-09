using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// ���� UI ���� ��ũ��Ʈ
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

    private int[] rand = { 0, 0, 0, 0, 0 };

    List<Dictionary<string, object>> data;

    private void Awake()
    {
        data = CSVReader.Read("Questions");
        /*for (var i = 0; i < data.Count; i++)
        {
            Debug.Log("Num " + data[i]["Num"] + " " +
                   "Question " + data[i]["Question"] + " " +
                   "Answer " + data[i]["Answer"]);
        }*/

        rand = UniqueRandom(5, 0, 9);

        for(int i = 0; i < rand.Length; i++)
        {
            Debug.Log("rand[i] : " + rand[i]);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        QuizNum = 0;
        score = 0;
        date = "";

        text_check.text = "";
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
        QuizNum = 0;
        score = 0;

        NextQuestion(); 
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

        // �����̸�
        if (answer == int.Parse(data[rand[QuizNum - 1]]["Answer"].ToString()))
        {
            text_check.text = "O";
            StartCoroutine(ShowCheckAnswer());
            score = score + 20;
        }

        // �����̸�

        else
        {
            text_check.text = "X";
            StartCoroutine(ShowCheckAnswer());
        }
         
    }

    private void NextQuestion()
    {
        QuizNum++;

        // ��� ���� ��
        if (QuizNum == 6)
        {
            text_title.text = "";
            text_queistion.text = "������!\n" + score.ToString() + "���� �޾Ҿ�!";
            text_O.text = "�ٽ� �ҷ�!";
            text_X.text = "����������!";

            // ���� �ð� �� ���� ���
            date = DateTime.Now.ToString(("yyyy.MM.dd HH:mm"));
            Debug.Log(date);
        }

        else
        {
            // ���� & �� ����

            text_queistion.text = data[rand[QuizNum-1]]["Question"].ToString();
            text_O.text = "�¾�!";
            text_X.text = "�ƴϾ�!";

            text_title.text = QuizNum.ToString() + "������";
        }
    }

    private IEnumerator ShowCheckAnswer()
    {
        text_check.color = new Color(text_check.color.r, text_check.color.g, text_check.color.b, 1);
        while (text_check.color.a > 0.0f)
        {
            text_check.color = new Color(text_check.color.r, text_check.color.g, text_check.color.b, text_check.color.a - (Time.deltaTime / 0.8f));
            yield return null;
        }

        btn_O.enabled = true;
        btn_X.enabled = true;

        NextQuestion();
    }

    // �ߺ� ���� ���� ����
    private int[] UniqueRandom(int count, int min, int max)
    {

        int[] rand = new int[count];
        int[] range = new int[max - min + 1];

        // �迭 �ʱ�ȭ
        for (int i = 0; i < range.Length; i++)
        {
            range[i] = min + i;
        }

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(i, range.Length);
            rand[i] = range[randomIndex];

            // ���� ���ڿ� ������ ���� ��ȯ
            range[randomIndex] = range[i];
            range[i] = rand[i];
        }

        return rand;
    }
}
