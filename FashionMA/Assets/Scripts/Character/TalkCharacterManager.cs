using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 대화창 관련 스크립트
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

    // 흥미로운 정보 csv
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
        text_talk.text = "무엇이 궁금해?";

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

        string tempInfo = "오늘의 최고 기온은 " + UniteData.tmn + "도,\n최저 기온은 " + UniteData.tmx + "도야.\n";
        bool rain = false; //비 예정이면 true
        for (int i=0; i<UniteData.todayWeather.GetLength(0); i++)
        {
            if(UniteData.todayWeather[i,1] != "0")
            {
                rain = true;
                break;
            }
        }
        string rainInfo;
        if (rain) rainInfo = "오늘 비 소식은 있어!";
        else rainInfo = "오늘 비 소식은 없어!";
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
        string first = (value + 1).ToString() + "시의 기온은 " + UniteData.todayWeather[value, 6] + "도, 습도는 " + UniteData.todayWeather[value, 3] + "%야.\n";
        
        string second;
        if(UniteData.todayWeather[value, 1] == "1")
        {
            //second = UniteData.todayWeather[value, 0] + "% 의 확률로 비가 올 예정이고, 강수량은 " + UniteData.todayWeather[value, 2] + "mm 이야!\n";
            second = "비가 올 예정이야!\n";
        }
        else if (UniteData.todayWeather[value, 1] == "2")
        {
            second = "눈과 비가 함께 올 예정이야!\n";
        }
        else if (UniteData.todayWeather[value, 1] == "3")
        {
            second = "눈이 올 예정이야!\n";
        }
        else if (UniteData.todayWeather[value, 1] == "4")
        {
            second = "소나기가 올 예정이야!\n";
        }
        else
        {
            second = "눈이나 비 소식은 없어!\n";
        }

        string third;
        if (UniteData.todayWeather[value, 5] == "3")
        {
            //third = (value + 1).ToString() + "시는 구름이 많고, 풍속은 " + UniteData.todayWeather[value, 11] + "m/s 야!\n";
            third = (value + 1).ToString() + "시는 구름이 많을 거야.\n";
        }
        else if (UniteData.todayWeather[value, 5] == "4")
        {
            //third = (value + 1).ToString() + "시는 흐리고, 풍속은 " + UniteData.todayWeather[value, 11] + "m/s 야!\n";
            third = (value + 1).ToString() + "시는 흐릴 거야.\n";
        }
        else
        {
            //third = (value + 1).ToString() + "시는 맑고, 풍속은 " + UniteData.todayWeather[value, 11] + "m/s 야!\n";
            third = (value + 1).ToString() + "시는 맑을 거야.\n";
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
