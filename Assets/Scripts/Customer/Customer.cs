using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Customer_Movement))]
[RequireComponent(typeof(Customer_UI))]
public class Customer : MonoBehaviour
{
    public SO_Customer customerData;
    [Header("==== Orders ====")]
    public SO_Order[] orders; // รายการอาหารที่ลูกค้าสั่ง

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
