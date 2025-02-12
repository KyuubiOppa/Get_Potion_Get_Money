using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Customer_Movement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform target; // เป้าหมายที่ลูกค้าจะไป
    public List<Customer_Movement> customersInQueue = new List<Customer_Movement>(); // ลูกค้าที่อยู่ในคิว

    void Start()
    {
        FindAvailableTarget(); // ค้นหาเป้าหมายที่ว่างเมื่อเริ่มเกม
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    /// <summary>
    /// ค้นหาตำแหน่งที่ว่างและกำหนดให้ลูกค้า
    /// </summary>
    void FindAvailableTarget()
    {
        if (CustomerManager.Instance == null) return;

        Transform[] holdPoints = CustomerManager.Instance.customerHoldPoint;
        List<Customer_Movement> allCustomers = FindObjectsOfType<Customer_Movement>().ToList();

        // วนลูปจากจุดสุดท้ายไปยังจุดแรก
        for (int i = holdPoints.Length - 1; i >= 0; i--)
        {
            Transform point = holdPoints[i];

            // เช็คว่ามีลูกค้าอยู่ที่จุดนี้หรือยัง
            bool isOccupied = allCustomers.Any(c => c.target == point);
            if (!isOccupied)
            {
                target = point; // กำหนดเป้าหมายที่ยังไม่มีลูกค้าใช้
                return;
            }
        }
    }

    /// <summary>
    /// เดินตาม Target
    /// </summary>
    void MoveTowardsTarget()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }
}
