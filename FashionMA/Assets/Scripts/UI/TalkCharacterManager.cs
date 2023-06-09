using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 대화창 관련 스크립트
public class TalkCharacterManager : MonoBehaviour
{
    [SerializeField] private GameObject icons;
    [SerializeField] private GameObject bottomMenu;
    [SerializeField] private GameObject talkPanels;

    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject choiceTalkPanel;
    [SerializeField] private GameObject nextBtn;

    // 흥미로운 정보 csv
    List<Dictionary<string, object>> data;
    private int rand = 0;
    [SerializeField] private Text text_talk;

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
        bottomMenu.SetActive(false);
        choicePanel.SetActive(true);
        talkPanels.SetActive(true);
    }

    public void SetFirst()
    { 
        icons.SetActive(true);
        bottomMenu.SetActive(true);
        talkPanels.SetActive(false);
        choiceTalkPanel.SetActive(false);
        nextBtn.SetActive(true);
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
}
