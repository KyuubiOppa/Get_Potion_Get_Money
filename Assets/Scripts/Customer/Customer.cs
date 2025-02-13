using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Customer_Movement))]
[RequireComponent(typeof(Customer_UI))]
public class Customer : MonoBehaviour
{
    public SO_Customer customerData;

    [Header("==== Orders ====")]
    int maxOrder = 4; // จำนวนคำสั่งซื้อสูงสุด
    public SO_Order[] orders; // รายการอาหารที่ลูกค้าสั่ง
    public int orderPrices; // รวมราคา Order ที่ลูกค้าสั่ง
    [SerializeField] private OrderServe orderServe; // อ้างอิงถึง OrderServe
    void Start()
    {
        orderServe = FindObjectOfType<OrderServe>();

        RandomOrder(); // สุ่มคำสั่งซื้อเมื่อเริ่มต้น
        CalculateOrderPrices();
    }

    void Update()
    {
        // อัปเดตตามความต้องการ
    }

/// <summary>
/// คำนวนราคา Order ที่สั่ง
/// </summary>
    public void CalculateOrderPrices()
    {
        orderPrices = 0; // รีเซ็ตค่าราคารวมก่อนคำนวณใหม่

        if (orders != null && orders.Length > 0)
        {
            // วนลูปผ่านทุก Order ใน orders
            foreach (SO_Order order in orders)
            {
                if (order != null)
                {
                    orderPrices += order.orderPrice; // เพิ่มราคาของ Order เข้าไปใน orderPrices
                }
            }
        }
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
    }

    /// <summary>
    /// เช็ครายการใน orderInCounters ว่ามีรายการที่ตรงกับ orders หรือไม่ และลดรายการที่ตรงกัน
    /// </summary>
    public void GetOrder()
    {
        if (orderServe != null)
        {
            // สร้าง Dictionary เพื่อนับจำนวนออเดอร์ที่ซ้ำกันใน orders
            Dictionary<SO_Order, int> orderCounts = new Dictionary<SO_Order, int>();
            foreach (var order in orders)
            {
                if (order != null)
                {
                    if (orderCounts.ContainsKey(order))
                    {
                        orderCounts[order]++;
                    }
                    else
                    {
                        orderCounts[order] = 1;
                    }
                }
            }

            // วนลูปตรวจสอบออเดอร์ใน orderInCounters
            foreach (var order in orderServe.orderInCounters.ToList()) // ใช้ ToList() เพื่อป้องกันการแก้ไขลิสต์ขณะวนลูป
            {
                if (orderCounts.ContainsKey(order) && orderCounts[order] > 0)
                {
                    // ลดจำนวนออเดอร์ที่ตรงกัน
                    orderCounts[order]--;
                    ReduceOrder(order); // ลดออเดอร์ใน orders
                    orderServe.orderInCounters.Remove(order); // ลบออเดอร์ใน orderInCounters

                    // ทำลาย GameObject ที่เกี่ยวข้องกับออเดอร์ที่รับไปแล้ว
                    orderServe.DestroyOrderObjects(order);
                }
            }

            Debug.Log("เสิร์ฟออเดอร์สำเร็จ");

            // ตรวจสอบว่าออเดอร์ทั้งหมดถูกเสิร์ฟครบหรือไม่
            if (IsAllOrdersServed())
            {
                // จ่ายเงินให้ผู้เล่น
                PayToPlayer();

                // เดินไปยังจุดออก
                Customer_Movement movement = GetComponent<Customer_Movement>();
                if (movement != null)
                {
                    movement.MoveToExit();
                }
            }
        }
    }

    /// <summary>
    /// ตรวจสอบว่าออเดอร์ทั้งหมดถูกเสิร์ฟครบหรือไม่
    /// </summary>
    private bool IsAllOrdersServed()
    {
        foreach (var order in orders)
        {
            if (order != null)
            {
                return false; // ถ้ายังมีออเดอร์ที่ยังไม่เสิร์ฟ
            }
        }
        return true; // ถ้าออเดอร์ทั้งหมดถูกเสิร์ฟครบ
    }

    /// <summary>
    /// จ่ายเงินให้ผู้เล่น
    /// </summary>
    private void PayToPlayer()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerMoney += orderPrices; // เพิ่มเงินให้ผู้เล่นตามราคารวมของออเดอร์
            Debug.Log("ลูกค้าจ่ายเงินให้ผู้เล่น: " + orderPrices + " บาท");
        }
    }

#region ลดรายการ Order

    /// <summary>
    /// ลดจำนวนออเดอร์ที่ตรงกันใน orders
    /// </summary>
    private void ReduceOrder(SO_Order orderToReduce)
    {
        for (int i = 0; i < orders.Length; i++)
        {
            if (orders[i] == orderToReduce)
            {
                ReduceOrderAtIndex(i);
                break; // หยุดหลังจากลดออเดอร์หนึ่งครั้ง
            }
        }
    }

    public void ReduceOrderAtIndex(int index)
    {
        if (index >= 0 && index < orders.Length && orders[index] != null)
        {
            // เลื่อนออเดอร์ที่เหลือมาแทนที่ออเดอร์ที่ถูกเสิร์ฟ
            for (int i = index; i < orders.Length - 1; i++)
            {
                orders[i] = orders[i + 1];
            }
            orders[orders.Length - 1] = null; // เคลียร์ออเดอร์สุดท้าย
        }
    }
#endregion
}