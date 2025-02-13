using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OrderServe : MonoBehaviour
{
    [Header("Order ที่มีบน Counter")]
    public List<SO_Order> orderInCounters = new List<SO_Order>();

    [Header("ตำแหน่งปล่อย Order Prefabs")]
    public Transform[] orderOutputDropPoints; // ตำแหน่งปล่อย Output ออกมา
    public List<Order_Object> currentOrderObjs = new List<Order_Object>(); // เปลี่ยนเป็น List<Order_Object>

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    /// <summary>
    /// ใช้สำหรับทิ้งลงถังขยะ
    /// </summary>
    public void ClearCurrentOrderServeAll()
    {
        if (orderInCounters.Count > 0 || currentOrderObjs.Count > 0)
        {
            // ทำลาย GameObject ที่อยู่ใน currentOrderObjs
            foreach (var orderObj in currentOrderObjs)
            {
                if (orderObj != null)
                {
                    Destroy(orderObj.gameObject); // ทำลาย GameObject ของ Order_Object
                }
            }

            // เคลียร์ลิสต์ของ Order_Object และ SO_Order
            currentOrderObjs.Clear();
            orderInCounters.Clear();
        }
    }

    /// <summary>
    /// ทำลาย GameObject ที่มี SO_Order ที่ตรงกัน
    /// </summary>
    public void DestroyOrderObjects(SO_Order orderToDestroy)
    {
        // วนลูปผ่าน currentOrderObjs เพื่อหา Order_Object ที่มี SO_Order ที่ตรงกัน
        foreach (var orderObj in currentOrderObjs.ToList()) // ใช้ ToList() เพื่อป้องกันการแก้ไขลิสต์ขณะวนลูป
        {
            if (orderObj != null && orderObj.so_Order == orderToDestroy)
            {
                // ทำลาย GameObject ของ Order_Object
                Destroy(orderObj.gameObject);
                currentOrderObjs.Remove(orderObj); // ลบออกจากลิสต์
                break; // หยุดหลังจากทำลาย Order_Object ตัวแรกที่พบ
            }
        }
    }

    public void AddOrderObjectInTable()
    {
        // เคลียร์ลิสต์ currentOrderObjs ก่อนเริ่มต้น
        currentOrderObjs.Clear();

        // วนลูปผ่านทุกตำแหน่งใน orderOutputDropPoints
        foreach (Transform dropPoint in orderOutputDropPoints)
        {
            // ตรวจสอบว่าตำแหน่ง dropPoint มีลูก (child) หรือไม่
            if (dropPoint.childCount > 0)
            {
                // ดึง GameObject ลูกตัวแรกของ dropPoint
                GameObject childObj = dropPoint.GetChild(0).gameObject;

                // ดึงคอมโพเนนต์ Order_Object จาก GameObject ลูก
                Order_Object orderObject = childObj.GetComponent<Order_Object>();

                // ถ้าเจอ Order_Object ให้เพิ่มเข้าไปใน currentOrderObjs
                if (orderObject != null)
                {
                    currentOrderObjs.Add(orderObject);
                }
            }
        }

        Debug.Log("เพิ่ม Order_Object จาก orderOutputDropPoints ลงใน currentOrderObjs เรียบร้อยแล้ว!");
    }
}