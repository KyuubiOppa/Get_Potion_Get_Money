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

    [Header("Color Combo Canvas")]
    public TMP_Text colorComboText;
    [Header("Color")]
    public Color red;
    public Color yellow;
    public Color blue;
    public Color orange;
    public Color green;
    public Color purple;
    public Color brown;

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
        UpdateColorComboText();
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

/// <summary>
/// อัปเดตข้อความและสีของ colorComboText ตาม Combo ปัจจุบัน
/// </summary>
    private void UpdateColorComboText()
    {
        if (ColorComboManager.Instance != null)
        {
            // ตั้งค่าข้อความ Combo (เช่น "x1", "x2", "x3")
            colorComboText.text = "x" + ColorComboManager.Instance.colorCombo;

            // เปลี่ยนสีตัวอักษรตาม colorComboState
            switch (ColorComboManager.Instance.colorComboState)
            {
                case ColorPotion.red:
                    colorComboText.color = red;
                    colorComboText.gameObject.SetActive(true);
                    break;
                case ColorPotion.yellow:
                    colorComboText.color = yellow;
                    colorComboText.gameObject.SetActive(true);
                    break;
                case ColorPotion.blue:
                    colorComboText.color = blue;
                    colorComboText.gameObject.SetActive(true);
                    break;
                case ColorPotion.orange:
                    colorComboText.color = orange;
                    colorComboText.gameObject.SetActive(true);
                    break;
                case ColorPotion.green:
                    colorComboText.color = green;
                    colorComboText.gameObject.SetActive(true);
                    break;
                case ColorPotion.purple:
                    colorComboText.color = purple;
                    colorComboText.gameObject.SetActive(true);
                    break;
                case ColorPotion.brown:
                    colorComboText.color = brown;
                    colorComboText.gameObject.SetActive(true);
                    break;
                default:
                    colorComboText.color = Color.white; // สีเริ่มต้นหากไม่มีสีที่ตรงกัน
                    colorComboText.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
