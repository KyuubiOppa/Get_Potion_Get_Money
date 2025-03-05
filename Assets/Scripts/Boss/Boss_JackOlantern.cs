using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_JackOlantern : MonoBehaviour
{
    public SO_Customer customerData;
    [Header("==== ความอดทน ====")]
    public CustomerEmotional bossEmotional = CustomerEmotional.Happy;

    Boss_JackOlantern_Movement boss_JackOlantern_Movement;
    Boss_JackOlantern_UI boss_JackOlantern_UI;

    void Start()
    {
        boss_JackOlantern_Movement = GetComponent<Boss_JackOlantern_Movement>();
        boss_JackOlantern_UI = GetComponent<Boss_JackOlantern_UI>();
    }

    void Update()
    {

    }

/// <summary>
/// รับเงินจากผู้เล่น
/// /// </summary>
    public void GetPlayerMoney()
    {
        if (GameManager.Instance.playerMoney >= GameManager.Instance.moneyToNeed)
        {
            // หักเงิน
            GameManager.Instance.playerMoney -= GameManager.Instance.moneyToNeed;
            // Clear Gauge
            GameManager.Instance.ClearGaugeBoss();

            // ทำให้ตัวละครอยู่ด้านหลัง
            boss_JackOlantern_UI.bossSpriteRenderer.sortingOrder = 0;
            // ปิด Canvas
            boss_JackOlantern_UI.bossCanvas.SetActive(false);
            boss_JackOlantern_Movement.MoveToExit();
        }
    }
}
