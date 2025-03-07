using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Customer_Movement))]
[RequireComponent(typeof(Customer_UI))]
public class Customer : MonoBehaviour
{
    public SO_Customer customerData;

    [Header("==== ความอดทน ====")]
    public CustomerEmotional customerEmotional = CustomerEmotional.Happy;
    public float patience;

    [Header("==== Orders ====")]
    int maxOrder = 4; // จำนวนคำสั่งซื้อสูงสุด
    public SO_Order[] orders; // รายการอาหารที่ลูกค้าสั่ง
    public int orderPrices; // รวมราคา Order ที่ลูกค้าสั่ง
    [SerializeField] private OrderServe orderServe; // อ้างอิงถึง OrderServe

    Customer_Movement customer_Movement;
    Customer_UI customer_UI;

    void Start()
    {
        customer_UI = GetComponent<Customer_UI>();
        customer_Movement = GetComponent<Customer_Movement>();

        orderServe = FindObjectOfType<OrderServe>();

        patience = customerData.customerPatience;

        customer_UI.customerCanvas.SetActive(false);
        customer_UI.customerSpriteRenderer.sortingOrder = 0;

        RandomOrder(); // สุ่มคำสั่งซื้อเมื่อเริ่มต้น
        CalculateOrderPrices();
    }

    void Update()
    {
        if (customer_Movement.hasReachedHoldPoint)
        {
            PatienceCountdown();
        }
    }

#region ความอดทน
    public void PatienceCountdown()
    {
        // ลดความอดทนตามเวลา
        patience -= Time.deltaTime;
        patience = Mathf.Clamp(patience, 0, customerData.customerPatience);

        // ตรวจสอบว่าความอดทนหมดหรือไม่
        if (patience <= 0)
        {
            IsTimeOut();
        }
    }
#endregion

#region คำนวนราคา Order ที่สั่ง & จ่ายตัง

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
    /// จ่ายเงินให้ผู้เล่น
    /// </summary>
    private void PayToPlayer()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerMoney += orderPrices * ColorComboManager.Instance.colorCombo; // เพิ่มเงินให้ผู้เล่นตามราคารวมของออเดอร์
            Debug.Log("ลูกค้าจ่ายเงินให้ผู้เล่น: " + orderPrices + " บาท");
        }
    }
#endregion

#region สุ่ม Order & รับ Order
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

                    // เช็ค Combo ถ้า Order เป็นประเภท Potion
                    if (order.orderType == OrderType.Potion)
                    {
                        ColorComboManager.Instance.IncreaseCombo(order.colorPotion);
                        // สร้าง ColorCombo Canvas ใหม่ให้ลอยขึ้นมา
                        Instantiate(ColorComboManager.Instance.colorComboCanvasPrefab, transform.position, Quaternion.identity);
                    }
                }
            }

            Debug.Log("เสิร์ฟออเดอร์สำเร็จ");

            // ตรวจสอบว่าออเดอร์ทั้งหมดถูกเสิร์ฟครบหรือไม่
            if (IsAllOrdersServed())
            {
                IsGetAllOrders();
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
/// ถ้าเก็บ Order ครบแล้ว
/// </summary>
    void IsGetAllOrders()
    {
        // ทำให้ตัวละครอยู่ด้านหลัง
        customer_UI.customerSpriteRenderer.sortingOrder = 0;
        // ปิด Canvas
        customer_UI.customerCanvas.SetActive(false);
        

        // จ่ายเงินให้ผู้เล่น
        PayToPlayer();

        // เดินไปยังจุดออก
        customer_Movement.MoveToExit();
    }

/// <summary>
/// ถ้าหมดเวลา
/// </summary>
    void IsTimeOut()
    {
        // ทำให้ตัวละครอยู่ด้านหลัง
        customer_UI.customerSpriteRenderer.sortingOrder = 0;
        // ปิด Canvas
        customer_UI.customerCanvas.SetActive(false);

        // เดินไปยังจุดออก
        customer_Movement.MoveToExit();
    }
#endregion

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