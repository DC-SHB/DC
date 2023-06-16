using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��ȭâ ���� ��ũ��Ʈ
public class TalkCharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject icons;
    [SerializeField] private GameObject clock;
    [SerializeField] private GameObject bottomMenu;
    [SerializeField] private GameObject talkPanels;

    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choiceTalkPanel;
    [SerializeField] private GameObject choiceWeatherPanel;
    [SerializeField] private GameObject WeatherDetailPanel;
    [SerializeField] private GameObject nextBtn;
    [SerializeField] private Slider slider;

    // ��̷ο� ���� csv
    List<Dictionary<string, object>> data;
    private int rand = 0;
    [SerializeField] private Text text_talk;

    private bool showSlider = false;

    // Start is called before the first frame update
    void Start()
    {
        SetFirst();
        data = CSVReader.Read("Information");
        /*for (var i = 0; i < data.Count; i++)
        {
            Debug.Log("Num " + data[i]["Num"] + " " +
                   "Question " + data[i]["Script"]);
        }*/
    }

    public void BtnTalk()
    {
        text_talk.text = "������ �ñ���?";

        icons.SetActive(false);
        clock.SetActive(false);
        bottomMenu.SetActive(false);
        choicePanel.SetActive(true);
        talkPanels.SetActive(true);
    }

    public void SetFirst()
    { 
        icons.SetActive(true);
        clock.SetActive(true);
        bottomMenu.SetActive(true);
        talkPanels.SetActive(false);
        choiceTalkPanel.SetActive(false);
        choiceWeatherPanel.SetActive(false);
        WeatherDetailPanel.SetActive(false);
        nextBtn.SetActive(true);
        showSlider = false;
    }

    public void BtnRegion()
    { 
        
    }

    public void BtnInfo()
    {
        choicePanel.SetActive(false);
        choiceTalkPanel.SetActive(true);

        rand = Random.Range(0, 10);
        rand = rand * 2;
        Debug.Log("rand : " + rand);

        data[rand]["Script"] = data[rand]["Script"].ToString().Replace("  ", "\n");
        text_talk.text = data[rand]["Script"].ToString();
    }

    public void BtnNextdialogue()
    {
        rand = rand + 1;
        data[rand]["Script"] = data[rand]["Script"].ToString().Replace("  ", "\n");
        text_talk.text = data[rand]["Script"].ToString();

        nextBtn.SetActive(false);
    }

    public void BtnTodayWeather()
    {
        choicePanel.SetActive(false);
        choiceWeatherPanel.SetActive(true);

        string tempInfo = "������ �ְ� ����� " + UniteData.tmn + "��,\n���� ����� " + UniteData.tmx + "����.\n";
        bool rain = false; //�� �����̸� true
        for (int i=0; i<UniteData.todayWeather.GetLength(0); i++)
        {
            if(UniteData.todayWeather[i,1] != "0")
            {
                rain = true;
                break;
            }
        }
        string rainInfo;
        if (rain) rainInfo = "���� �� �ҽ��� �־�!";
        else rainInfo = "���� �� �ҽ��� ����!";
        text_talk.text = tempInfo + rainInfo;
    }

    public void BtnDetailWeather()
    {
        choicePanel.SetActive(false);
        choiceWeatherPanel.SetActive(false);
        WeatherDetailPanel.SetActive(true);
        showSlider = true;
    }

    public void ShowDetailWeather(int value)
    {
        string first = (value + 1).ToString() + "���� ����� " + UniteData.todayWeather[value, 6] + "��, ������ " + UniteData.todayWeather[value, 3] + "%��.\n";
        
        string second;
        if(UniteData.todayWeather[value, 1] == "1")
        {
            //second = UniteData.todayWeather[value, 0] + "% �� Ȯ���� �� �� �����̰�, �������� " + UniteData.todayWeather[value, 2] + "mm �̾�!\n";
            second = "�� �� �����̾�!\n";
        }
        else if (UniteData.todayWeather[value, 1] == "2")
        {
            second = "���� �� �Բ� �� �����̾�!\n";
        }
        else if (UniteData.todayWeather[value, 1] == "3")
        {
            second = "���� �� �����̾�!\n";
        }
        else if (UniteData.todayWeather[value, 1] == "4")
        {
            second = "�ҳ��Ⱑ �� �����̾�!\n";
        }
        else
        {
            second = "���̳� �� �ҽ��� ����!\n";
        }

        string third;
        if (UniteData.todayWeather[value, 5] == "3")
        {
            //third = (value + 1).ToString() + "�ô� ������ ����, ǳ���� " + UniteData.todayWeather[value, 11] + "m/s ��!\n";
            third = (value + 1).ToString() + "�ô� ������ ���� �ž�.\n";
        }
        else if (UniteData.todayWeather[value, 5] == "4")
        {
            //third = (value + 1).ToString() + "�ô� �帮��, ǳ���� " + UniteData.todayWeather[value, 11] + "m/s ��!\n";
            third = (value + 1).ToString() + "�ô� �帱 �ž�.\n";
        }
        else
        {
            //third = (value + 1).ToString() + "�ô� ����, ǳ���� " + UniteData.todayWeather[value, 11] + "m/s ��!\n";
            third = (value + 1).ToString() + "�ô� ���� �ž�.\n";
        }

        text_talk.text = first + second + third;
    }

    private void Update()
    {
        if(showSlider)
        {
            int value = (int)slider.value;
            ShowDetailWeather(value);
        }
    }
}
