using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorComboCanvas : MonoBehaviour
{
    public TMP_Text colorComboText;
    public float flySpeed = 1.0f;

    void Start()
    {
        UpdateComboText();
    }

    void Update()
    {
        FlyUpToDestroy();
    }

    public void FlyUpToDestroy()
    {
        transform.Translate(Vector3.up * flySpeed * Time.deltaTime);
        Destroy(gameObject, 1f);
    }

/// <summary>
/// อัปเดตข้อความและสีของ colorComboText ตาม Combo ปัจจุบัน
/// </summary>
    private void UpdateComboText()
    {
        if (ColorComboManager.Instance != null)
        {
            // ตั้งค่าข้อความ Combo (เช่น "x1", "x2", "x3")
            colorComboText.text = "x" + ColorComboManager.Instance.colorCombo;

            // เปลี่ยนสีตัวอักษรตาม colorComboState
            switch (ColorComboManager.Instance.colorComboState)
            {
                case ColorPotion.red:
                    colorComboText.color = UIManager.Instance.red;
                    break;
                case ColorPotion.yellow:
                    colorComboText.color = UIManager.Instance.yellow;
                    break;
                case ColorPotion.blue:
                    colorComboText.color = UIManager.Instance.blue;
                    break;
                case ColorPotion.orange:
                    colorComboText.color = UIManager.Instance.orange;
                    break;
                case ColorPotion.green:
                    colorComboText.color = UIManager.Instance.green;
                    break;
                case ColorPotion.purple:
                    colorComboText.color = UIManager.Instance.purple;
                    break;
                case ColorPotion.brown:
                    colorComboText.color = UIManager.Instance.brown;
                    break;
                default:
                    colorComboText.color = Color.white; // สีเริ่มต้นหากไม่มีสีที่ตรงกัน
                    break;
            }
        }
    }
}
