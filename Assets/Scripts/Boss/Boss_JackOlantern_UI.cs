using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss_JackOlantern_UI : MonoBehaviour
{
    [Header("ปุ่ม Player กดเพื่อให้เงิน")]
    public Button getPlayerMoneyButton; // ปุ่มที่ผู้เล่นกดเพื่อให้เงิน

    [Header("Canvas Ui ทั้งหมด")]
    public GameObject bossCanvas;
    [Header("รูปตัวละคร")]
    public SpriteRenderer bossSpriteRenderer; // แสดงรูปลูกค้า

    [Header("เงิน Text")]
    public TMP_Text needMoneyText;

    Boss_JackOlantern boss_JackOlantern;

    void Start()
    {
        boss_JackOlantern = GetComponent<Boss_JackOlantern>();

        getPlayerMoneyButton.onClick.AddListener(boss_JackOlantern.GetPlayerMoney);
    }

    void Update()
    {
        BossEmotional();
        NeedMoneyText();
    }

    public void BossEmotional()
    {
        if (boss_JackOlantern == null || boss_JackOlantern.customerData == null || bossSpriteRenderer == null)
        {
            return;
        }

        if (GameManager.Instance.currentBossGauge >= 225)
        {
            bossSpriteRenderer.sprite = boss_JackOlantern.customerData.customerSmile;
            boss_JackOlantern.bossEmotional = global::CustomerEmotional.Happy;
        }
        if (GameManager.Instance.currentBossGauge >= 250)
        {
            bossSpriteRenderer.sprite = boss_JackOlantern.customerData.customerTight;
            boss_JackOlantern.bossEmotional = global::CustomerEmotional.Normal;
        }
        if (GameManager.Instance.currentBossGauge >= 275)
        {
            bossSpriteRenderer.sprite = boss_JackOlantern.customerData.customerAngry;
            boss_JackOlantern.bossEmotional = global::CustomerEmotional.Angry;
        }
    }

    public void NeedMoneyText()
    {
        needMoneyText.text = GameManager.Instance.moneyToNeed.ToString() + " B";
    }
}
