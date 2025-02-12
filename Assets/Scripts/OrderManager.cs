using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

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

    [Header("Order ที่ลูกค้าจะสุ่ม")]
    public SO_Order[] randomOrders;

    void Start()
    {
        if (randomOrders == null || randomOrders.Length == 0)
        {
            Debug.LogWarning("ไม่มีรายการอาหารใน OrderManager");
        }
    }

    void Update()
    {
        
    }
}