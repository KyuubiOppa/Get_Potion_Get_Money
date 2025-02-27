using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Customer_Movement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform target; // เป้าหมายที่ลูกค้าจะไป

    Customer customer;
    Customer_UI customer_UI;

    public bool hasReachedHoldPoint = false;
    void Start()
    {
        customer = GetComponent<Customer>();
        customer_UI = GetComponent<Customer_UI>();

        FindAvailableTarget(); // ค้นหาเป้าหมายที่ว่างเมื่อเริ่มเกม
    }

    void Update()
    {
        MoveTowardsTarget();

        // ตรวจสอบว่าลูกค้าเดินมาถึงจุด HoldPoint หรือยัง
        if (!hasReachedHoldPoint && target != null && Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            hasReachedHoldPoint = true;
            customer_UI.customerSpriteRenderer.sortingOrder = 1;
            customer_UI.customerCanvas.SetActive(true);
        }

        // ถ้าเป้าหมายคือจุดออก และถึงจุดนั้นแล้ว
        if (target == CustomerManager.Instance.customerExitPoint && Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // ทำลายตัวเอง
            Destroy(gameObject);

            // เรียกใช้การสปอว์นลูกค้าใหม่
            CustomerManager.Instance.SpawnNextCustomer();
        }
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

    /// <summary>
    /// เดินไปยังจุดออก
    /// </summary>
    public void MoveToExit()
    {
        target = CustomerManager.Instance.customerExitPoint;
    }
}