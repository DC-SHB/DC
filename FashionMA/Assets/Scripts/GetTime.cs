using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetTime : MonoBehaviour
{
    private string time;
    private int hour;
    private float hoursToDegrees = 360f / 12f, minutesToDegrees = 360f / 60f, secondsToDegrees = 360f / 60f;
    private bool analog = true;

    [SerializeField] private GameObject canvas;
    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;
    public Text timeText;
    public Sprite[] sprite = new Sprite[5];
    public Transform hours, minutes, seconds;
    public GameObject clock, BG;
    // Start is called before the first frame update
    void Start()
    {
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        m_EventSystem = GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        hour = DateTime.Now.Hour;
        if (hour >= 0 && hour < 5)
        {
            BG.GetComponent<SpriteRenderer>().sprite = sprite[0];
        }
        if (hour >= 5 && hour < 9)
        {
            BG.GetComponent<SpriteRenderer>().sprite = sprite[1];
        }
        if (hour >= 9 && hour < 17)
        {
            BG.GetComponent<SpriteRenderer>().sprite = sprite[2];
        }
        if (hour >= 17 && hour < 21)
        {
            BG.GetComponent<SpriteRenderer>().sprite = sprite[3];
        }
        if (hour >= 21 && hour < 24)
        {
            BG.GetComponent<SpriteRenderer>().sprite = sprite[4];
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!analog)
            {
                m_PointerEventData = new PointerEventData(m_EventSystem);
                m_PointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                m_Raycaster.Raycast(m_PointerEventData, results);
                try
                {
                    GameObject ray = results[0].gameObject;
                    if (ray.CompareTag("Clock")) analog = !analog;
                }
                catch { }
            }

            if (analog)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "Clock")
                        analog = !analog;
                }
            }
        }

        if (analog)
        {
            clock.SetActive(true);
            timeText.gameObject.SetActive(false);
            TimeSpan timespan = DateTime.Now.TimeOfDay;
            hours.localRotation = Quaternion.Euler(0f, 0f, (float)timespan.TotalHours * -hoursToDegrees);
            minutes.localRotation = Quaternion.Euler(0f, 0f, (float)timespan.TotalMinutes * -minutesToDegrees);
            seconds.localRotation = Quaternion.Euler(0f, 0f, (float)timespan.TotalSeconds * -secondsToDegrees);
        }
        else
        {
            timeText.gameObject.SetActive(true);
            clock.SetActive(false);
            time = DateTime.Now.ToString(("MM¿ù ddÀÏ HH:mm"));
            timeText.text = time;
        }
    }
}
