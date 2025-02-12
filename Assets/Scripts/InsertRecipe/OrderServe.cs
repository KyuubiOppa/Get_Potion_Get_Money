using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderServe : MonoBehaviour
{
    [Header("Order ที่มีบน Counter")]
    public List<SO_Order> orderInCounters = new List<SO_Order>();

    [Header("ตำแหน่งปล่อย Order Prefabs")]
    public Transform[] orderOutputDropPoints; // ตำแหน่งปล่อย Output ออกมา
    public List<GameObject> currentOrderObjs = new List<GameObject>();
    void Start()
    {
        
    }
    void Update()
    {
        
    }

/// <summary>
/// ล้าง Order ที่มีใน orderInCounters และทำลาย GameObject ของ currentOrderObjs
/// </summary>
    public void ClearCurrentOrderServe()
    {
        if (orderInCounters.Count > 0 || currentOrderObjs.Count > 0)
        {
            // ทำลาย GameObject ที่อยู่ใน currentOrderObjs
            foreach (GameObject obj in currentOrderObjs)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }

            // เคลียร์ลิสต์ของ GameObject และ SO_Order
            currentOrderObjs.Clear();
            orderInCounters.Clear();

            Debug.Log("ล้าง Order บน Counter ทั้งหมดแล้ว!");
        }
        else
        {
            Debug.LogWarning("ไม่มี Order บน Counter ให้ล้าง!");
        }
    }
}
