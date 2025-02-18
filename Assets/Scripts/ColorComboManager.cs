using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorComboManager : MonoBehaviour
{
    public static ColorComboManager Instance;

    public ColorPotion colorComboState;
    public int colorCombo = 1; // ใช้แบบ x1 x2 x3 x4 ไปเรื่อยๆ

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
        
    }
}
