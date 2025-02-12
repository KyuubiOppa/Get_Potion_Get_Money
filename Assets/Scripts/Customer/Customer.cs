using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Customer_Movement))]
[RequireComponent(typeof(Customer_UI))]
public class Customer : MonoBehaviour
{
    public SO_Customer customerData;

    [Header("==== Orders ====")]
    int maxOrder = 4; // จำนวนคำสั่งซื้อสูงสุด
    public SO_Order[] orders; // รายการอาหารที่ลูกค้าสั่ง

    void Start()
    {
        RandomOrder(); // สุ่มคำสั่งซื้อเมื่อเริ่มต้น
    }

    void Update()
    {
        // อัปเดตตามความต้องการ
    }

    /// <summary>
    /// สุ่มจำนวนคำสั่งซื้อและรายการอาหาร
    /// </summary>
    public void RandomOrder()
    {
        // สุ่มจำนวนคำสั่งซื้อ (1 ถึง maxOrder)
        int orderCount = Random.Range(1, maxOrder + 1);
        orders = new SO_Order[orderCount]; // กำหนดขนาดอาร์เรย์ orders

        // สุ่มรายการอาหารจาก OrderManager
        if (OrderManager.Instance != null && OrderManager.Instance.randomOrders.Length > 0)
        {
            for (int i = 0; i < orderCount; i++)
            {
                // สุ่มรายการอาหารจาก randomOrders ใน OrderManager
                int randomIndex = Random.Range(0, OrderManager.Instance.randomOrders.Length);
                orders[i] = OrderManager.Instance.randomOrders[randomIndex];
            }
        }
        else
        {
            Debug.LogWarning("ไม่มีรายการอาหารใน OrderManager");
        }

        // แสดงผลลัพธ์ (สำหรับทดสอบ)
        Debug.Log("ลูกค้าสั่งอาหาร " + orderCount + " รายการ:");
        foreach (var order in orders)
        {
            Debug.Log("- " + order.orderName);
        }
    }
}