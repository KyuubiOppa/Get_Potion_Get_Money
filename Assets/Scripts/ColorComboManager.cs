using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorComboManager : MonoBehaviour
{
    public static ColorComboManager Instance;

    public ColorPotion colorComboState;
    public int colorCombo = 1; // ใช้แบบ x1 x2 x3 x4 ไปเรื่อยๆ

    public GameObject colorComboCanvasPrefab;

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

    /// <summary>
    /// เพิ่ม Combo เมื่อเสิร์ฟสีเดียวกัน
    /// </summary>
    public void IncreaseCombo(ColorPotion servedColor)
    {
        if (servedColor == colorComboState)
        {
            colorCombo++;
        }
        else
        {
            colorComboState = servedColor;
            colorCombo = 1;
        }
        Debug.Log("Combo: " + colorComboState + " x" + colorCombo);
    }

    /// <summary>
    /// รีเซ็ต Combo เมื่อเสิร์ฟสีอื่น
    /// </summary>
    public void ResetCombo()
    {
        colorComboState = ColorPotion.NoColor;
        colorCombo = 1;
        Debug.Log("Combo Reset");
    }
}