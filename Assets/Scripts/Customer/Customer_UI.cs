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
    [Header("Emotional Image")]
    public Image emotionImage;
    public Sprite happyIcon;
    public Sprite normalIcon;
    public Sprite angryIcon;


    Customer customer;

    void Start()
    {
        customer = GetComponent<Customer>();
        getOrderButton.onClick.AddListener(customer.GetOrder);
    }

    void Update()
    {
        UpdateOrderUI();
        PatienceSlider();
        CustomerEmotional();
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

/// <summary>
/// อัพเดตหลอดความอดทน
/// </summary>
    public void PatienceSlider()
    {
        patienceSlider.maxValue = customer.customerData.customerPatience;
        patienceSlider.value = customer.patience;
    }

/// <summary>
/// อัปเดต Sprite ของลูกค้าตามความอดทนที่เหลือ
/// </summary>
    public void CustomerEmotional()
    {
        if (customer == null || customer.customerData == null || customerSpriteRenderer == null)
        {
            Debug.LogWarning("Customer, customerData, หรือ customerSpriteRenderer ไม่ถูกกำหนด");
            return;
        }

        // คำนวณเปอร์เซ็นต์ความอดทนที่เหลือ
        float patiencePercentage = (customer.patience / customer.customerData.customerPatience) * 100;

        // กำหนด Sprite ตามเปอร์เซ็นต์ความอดทน
        if (patiencePercentage > 66)
        {
            customerSpriteRenderer.sprite = customer.customerData.customerSmile; // ยิ้ม
            emotionImage.sprite = happyIcon;
            customer.customerEmotional = global::CustomerEmotional.Happy;
        }
        else if (patiencePercentage > 33)
        {
            customerSpriteRenderer.sprite = customer.customerData.customerTight; // เครียด
            emotionImage.sprite = normalIcon;
            customer.customerEmotional = global::CustomerEmotional.Normal;
        }
        else
        {
            customerSpriteRenderer.sprite = customer.customerData.customerAngry; // โกรธ
            emotionImage.sprite = angryIcon;
            customer.customerEmotional = global::CustomerEmotional.Angry;
        }
    }
}
