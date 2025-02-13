using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer_UI : MonoBehaviour
{
    public Button getOrderButton;

    public SpriteRenderer customerSpriteRenderer; // แสดงรูปลูกค้า
    public Image[] orderImages; // แสดงรูป Order ที่ลูกค้าสั่ง
    public Slider patienceSlider; // แสดงความอดทนของลูกค้า

    public Customer customer;

    void Start()
    {
        customer = GetComponent<Customer>();
        getOrderButton.onClick.AddListener(customer.GetOrder);
    }

    void Update()
    {
        UpdateOrderUI();
    }

    /// <summary>
    /// อัปเดตรูปภาพของ orderImages ตาม orders ที่ลูกค้าสั่ง
    /// </summary>
    public void UpdateOrderUI()
    {
        if (customer == null || customer.orders == null || orderImages == null)
        {
            Debug.LogWarning("Customer, orders, หรือ orderImages ไม่ถูกกำหนด");
            return;
        }

        // วนลูปเพื่ออัปเดตรูปภาพใน orderImages
        for (int i = 0; i < orderImages.Length; i++)
        {
            if (i < customer.orders.Length && customer.orders[i] != null)
            {
                // แสดงรูปภาพของ Order
                orderImages[i].sprite = customer.orders[i].orderSprite;
                orderImages[i].enabled = true; // เปิดใช้งาน Image
            }
            else
            {
                // ปิดใช้งาน Image ถ้าไม่มี Order
                orderImages[i].enabled = false;
            }
        }
    }
}
