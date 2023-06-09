using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizScoreManager : MonoBehaviour
{
    [SerializeField] private GameObject[] scoreLines;

    // Start is called before the first frame update
    void Start()
    {
        UniteData.LoadLeaderboard();

        for (int i = 0; i < UniteData.maxEntries; i++)
        {
            scoreLines[i].SetActive(false);
        }

        PrintLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {

    }


    

    private void PrintLeaderboard()
    {
        Debug.Log("Length : " + UniteData.logScore.Length);
        for (int i = 0; i < UniteData.logScore.Length; i++)
        {
            scoreLines[i].gameObject.transform.Find("Score").GetComponent<Text>().text = UniteData.logScore[i].ToString();
            scoreLines[i].gameObject.transform.Find("date").GetComponent<Text>().text = UniteData.logDate[i].ToString();
            scoreLines[i].SetActive(true);
        }
    }
}