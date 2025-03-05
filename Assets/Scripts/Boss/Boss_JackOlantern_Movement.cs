using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boss_JackOlantern_Movement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform target; // เป้าหมายที่ Boss จะไป

    Boss_JackOlantern boss_JackOlantern;
    Boss_JackOlantern_UI boss_JackOlantern_UI;
    public bool hasReachedHoldPoint = false;

    void Start()
    {
        boss_JackOlantern = GetComponent<Boss_JackOlantern>();
        boss_JackOlantern_UI = GetComponent<Boss_JackOlantern_UI>();

        FindAvailableTarget(); // ค้นหาเป้าหมายเมื่อเริ่มต้น
    }

    void Update()
    {
        // ถ้ายังไม่มีเป้าหมายหรือเป้าหมายคือ customerNullPoint ให้ค้นหาเป้าหมายใหม่
        if (target == null || target == CustomerManager.Instance.customerNullPoint)
        {
            FindAvailableTarget();
        }

        MoveTowardsTarget();

        // ตรวจสอบว่า Boss เดินมาถึงจุด HoldPoint หรือยัง
        if (!hasReachedHoldPoint && target != null && Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            hasReachedHoldPoint = true;
            boss_JackOlantern_UI.bossSpriteRenderer.sortingOrder = 1;
            boss_JackOlantern_UI.bossCanvas.SetActive(true);

            // ส่งสัญญาณกลับไปที่ GameManager เพื่อเริ่มเพิ่ม Gauge อีกครั้ง
            GameManager.Instance.stopChartGauge = false;
        }

        // ถ้าเป้าหมายคือจุดออก และถึงจุดนั้นแล้ว
        if (target == CustomerManager.Instance.customerExitPoint && Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // ทำลายตัวเอง
            Destroy(gameObject);

            // เรียกใช้การสปอว์นลูกค้าใหม่
            CustomerManager.Instance.StartCoroutine(CustomerManager.Instance.StartCustomerSpawn());

            // Clear ค่าเพื่อเริ่มนับเกจใหม่
            GameManager.Instance.stopChartGauge = false;
            GameManager.Instance.isBossMeeting = false;
            GameManager.Instance.isBossSpawned = false;
        }
    }

    /// <summary>
    /// ค้นหาตำแหน่งที่ว่างและกำหนดให้ Boss
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

        // ถ้าไม่มีจุด HoldPoint ที่ว่าง ให้ตั้งเป้าหมายเป็น null และรอ
        target = null;
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