using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public int day = 1; // วันที่ปัจจุบัน
    public float dayDuration = 300f; // ระยะเวลา 1 วัน (หน่วย: วินาที)

    // ตัวแปรเก็บเวลาในเกมที่อิงกับ Time.deltaTime (ขึ้นกับ Time.timeScale)
    public float currentGameTime = 0f;

    // ตัวแปรเก็บเวลาจริงที่อิงกับ Time.unscaledDeltaTime (ไม่ขึ้นกับ Time.timeScale)
    [SerializeField] private float currentRealTime = 0f;

    // ตัวแปรเก็บสถานะ Pause
    private bool isPaused = false;

    void Awake()
    {
        // ตั้งค่า Singleton
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
        // เริ่มต้นเวลา
        currentGameTime = 0f;
        currentRealTime = 0f;
    }

    void Update()
    {
        // อัปเดตเวลาในเกม (ขึ้นกับ Time.timeScale)
        if (!isPaused)
        {
            currentGameTime += Time.deltaTime;
            CheckDayPassed(); // ตรวจสอบว่าครบ 1 วันหรือไม่
        }

        // อัปเดตเวลาจริง (ไม่ขึ้นกับ Time.timeScale)
        currentRealTime += Time.unscaledDeltaTime;
    }

    /// <summary>
    /// ตรวจสอบว่าครบ 1 วันหรือไม่
    /// </summary>
    private void CheckDayPassed()
    {
        if (currentGameTime >= dayDuration)
        {
            currentGameTime = 0f; // รีเซ็ตเวลา
            day++; // เพิ่มวันที่

            // เรียกเมธอด RandomCustomerRate จาก CustomerManager
            if (CustomerManager.Instance != null)
            {
                CustomerManager.Instance.RandomCustomerRate();
            }

            Debug.Log("วันที่ใหม่: " + day);
        }
    }

    /// <summary>
    /// สลับสถานะ Pause/Resume เกม
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused; // สลับสถานะ Pause

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    /// <summary>
    /// หยุดเกม
    /// </summary>
    private void PauseGame()
    {
        Time.timeScale = 0f; // หยุดเวลาในเกม
        Debug.Log("เกมหยุด (Paused)");
    }

    /// <summary>
    /// เริ่มเกมใหม่
    /// </summary>
    private void ResumeGame()
    {
        Time.timeScale = 1f; // เริ่มเวลาในเกมใหม่
        Debug.Log("เกมทำงานต่อ (Resumed)");
    }
}