using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("======== CheckMark ========")]
    public bool isGameOver = false;

    [Header("======== บอส ========")]
    public float maxBossGauge = 300f;
    public float currentBossGauge;
    public int moneyToNeed = 100; // เงินที่ต้องการจากผู้เล่น
    public BossPhase bossPhase = BossPhase.Phase1;
    public bool stopChartGauge = false;
    public bool isBossMeeting = false; // ตอนที่บอสออกมา
    public bool isBossSpawned = false;
    [Header("เงินของผู้เล่น")]
    public int playerMoney = 0;

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

    void Update()
    {
        // บอสมาทวงตัง
        if (!isGameOver)
            BossChartGauge();
    
        BossGaugeSlider();
    }

/// <summary>
/// บอสชาร์จเกจ
/// </summary>
    public void BossChartGauge()
    {
        // หยุดเพิ่ม Gauge ถ้า stopChartGauge เป็น true
        if (stopChartGauge)
            return;

        // จำกัดค่าเกจไม่ให้เกิน maxBossGauge
        currentBossGauge = Mathf.Clamp(currentBossGauge + Time.deltaTime, 0, maxBossGauge);

        if (currentBossGauge >= maxBossGauge)
        {
            isGameOver = true;
            Debug.Log("Game Over");
            return;
        }

        // ใช้ Mathf.FloorToInt เพื่อให้ phaseLevel เป็นค่าจำนวนเต็มที่ถูกต้อง
        int phaseLevel = Mathf.FloorToInt(currentBossGauge / 75);

        switch (phaseLevel)
        {
            case 3:
                bossPhase = BossPhase.Phase4;
                isBossMeeting = true;
                if (!isBossSpawned) // ตรวจสอบว่า Boss ยังไม่ถูกเรียก
                {
                    CustomerManager.Instance.SpawnBoss();
                    isBossSpawned = true; // ตั้งค่าให้ Boss ถูกเรียกแล้ว
                    stopChartGauge = true; // หยุดเพิ่ม Gauge
                }
                break;
            case 2:
                bossPhase = BossPhase.Phase3;
                isBossMeeting = false;
                break;
            case 1:
                bossPhase = BossPhase.Phase2;
                isBossMeeting = false;
                break;
            default:
                bossPhase = BossPhase.Phase1;
                isBossMeeting = false;
                break;
        }
    }

/// <summary>
/// อัปเดต UI บอสเกจ
/// </summary>
    public void BossGaugeSlider()
    {
        UIManager.Instance.bossGaugeSlider.maxValue = maxBossGauge;
        UIManager.Instance.bossGaugeSlider.value = currentBossGauge;
    }

    public void ClearGaugeBoss()
    {
        currentBossGauge = 0f;
    }
}

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3,
    Phase4
}
