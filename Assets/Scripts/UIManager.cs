using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text playerMoneyTMP;


    public TMP_Text timeTMP;
    public TMP_Text dayTMP;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        PlayerMoney();
        DayAndTime();
    }

    public void PlayerMoney()
    {
        playerMoneyTMP.text = GameManager.Instance.playerMoney.ToString() + " B";
    }

    public void DayAndTime()
    {
        if (TimeManager.Instance == null)
        {
            Debug.LogWarning("TimeManager.Instance ไม่ถูกกำหนด");
            return;
        }

        // เวลาทั้งหมดในเกม (0 - 24 ชั่วโมง)
        float timeOfDay = (TimeManager.Instance.currentGameTime / TimeManager.Instance.dayDuration) * 24f;

        // แปลงเป็นชั่วโมงและนาที
        int hours = Mathf.FloorToInt(timeOfDay);
        int minutes = Mathf.FloorToInt((timeOfDay - hours) * 60);

        // จัดรูปแบบเป็น HH:MM (เช่น 08:30)
        string timeString = string.Format("{0:00}:{1:00}", hours, minutes);

        // อัปเดต UI
        timeTMP.text = timeString;
        dayTMP.text = "Day " + TimeManager.Instance.day.ToString();
    }
}
