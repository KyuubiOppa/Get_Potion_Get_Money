using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Header("ตัวปล่อย Boss")]
    public SO_Customer bossJackOlantern;

    [Header("ตัวสุ่มปล่อยลูกค้า")]
    public RandomCustomer[] randomCustomers;

    [Header("ตัวสุ่มปล่อยลูกค้า Special")]
    public RandomCustomerSpecial[] randomCustomerSpecials;

    [Header("ระยะเวลาห่างในการสร้างลูกค้า")]
    public float spawnInterval = 2f; // เวลาห่างระหว่างการสร้างลูกค้าแต่ละตัว

    [Header("จุดลูกค้าเกิด")]
    public Transform customerSpawnPoint;
    [Header("จุดลูกค้าออก")]
    public Transform customerExitPoint;

    [Header("จุดลูกค้ายืน")]
    public Transform[] customerHoldPoint; // จุดที่ลูกค้ายืน
    [Header("จุดลูกค้าไม่มีที่ไป")]
    public Transform customerNullPoint;

    private List<GameObject> spawnedCustomers = new List<GameObject>(); // เก็บลูกค้าที่ถูกสร้างขึ้นมา

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        RandomCustomerRate();
        RandomCustomerSpecialRate();
        StartCoroutine(StartCustomerSpawn());
    }

    void Update()
    {

    }

    /// <summary>
    /// ปล่อยลูกค้าตอนเริ่มเกม
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartCustomerSpawn()
    {
        if (randomCustomers.Length == 0 || customerSpawnPoint == null)
        {
            Debug.LogWarning("ไม่มีข้อมูลลูกค้าหรือจุดเกิดลูกค้า");
            yield break;
        }

        for (int i = customerHoldPoint.Length - 1; i >= 0; i--)
        {
            SO_Customer randomCustomer = GetRandomCustomer();
            if (randomCustomer != null)
            {
                GameObject newCustomer = Instantiate(randomCustomer.customerPrefab, customerSpawnPoint.position, Quaternion.identity);
                spawnedCustomers.Add(newCustomer);

                // รอระยะเวลาห่างก่อนสร้างลูกค้าตัวต่อไป
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    /// <summary>
    /// ปล่อยลูกค้าใหม่เพียงตัวเดียว
    /// </summary>
    public void SpawnNextCustomer()
    {
        if (randomCustomers.Length == 0 || customerSpawnPoint == null)
        {
            Debug.LogWarning("ไม่มีข้อมูลลูกค้าหรือจุดเกิดลูกค้า");
            return;
        }

        // สุ่มลูกค้าใหม่
        SO_Customer randomCustomer = GetRandomCustomer();
        if (randomCustomer != null)
        {
            GameObject newCustomer = Instantiate(randomCustomer.customerPrefab, customerSpawnPoint.position, Quaternion.identity);
            spawnedCustomers.Add(newCustomer);
            Debug.Log("สปอว์นลูกค้าใหม่: " + randomCustomer.name);
        }
    }

    /// <summary>
    /// คืนค่า SO_Customer แบบสุ่มตาม customerRate
    /// </summary>
    /// <returns>SO_Customer</returns>
    private SO_Customer GetRandomCustomer()
    {
        float totalRate = 0f;
        foreach (var customer in randomCustomers)
        {
            totalRate += customer.customerRate;
        }

        float randomValue = Random.Range(0f, totalRate);
        float cumulative = 0f;

        foreach (var customer in randomCustomers)
        {
            cumulative += customer.customerRate;
            if (randomValue <= cumulative)
            {
                return customer.so_Customer;
            }
        }

        return null;
    }

    /// <summary>
    /// สุ่ม customerRate ของ RandomCustomer แต่ละ element
    /// </summary>
    public void RandomCustomerRate()
    {
        foreach (var customer in randomCustomers)
        {
            customer.customerRate = Random.Range(0.1f, 1f); // สุ่มค่า customerRate ระหว่าง 0 ถึง 1
        }
    }

    /// <summary>
    /// สุ่ม customerRate ของ RandomCustomer แต่ละ element
    /// </summary>
    public void RandomCustomerSpecialRate()
    {
        foreach (var customerSpecial in randomCustomerSpecials)
        {
            customerSpecial.customerRate = Random.Range(0.1f, 1f); // สุ่มค่า customerRate ระหว่าง 0 ถึง 1
        }
    }

    public void SpawnBoss()
    {
        if (bossJackOlantern != null && bossJackOlantern.customerPrefab != null)
        {
            GameObject boss = Instantiate(bossJackOlantern.customerPrefab, customerSpawnPoint.position, Quaternion.identity);
            Debug.Log("Boss ถูกเรียกแล้ว!");
        }
        else
        {
            Debug.LogWarning("ไม่มีข้อมูล Boss หรือ Prefab ของ Boss");
        }
    }
}

[System.Serializable]
public class RandomCustomer
{
    public SO_Customer so_Customer;
    [Range(0f,1f)] public float customerRate = 1f; // 1f = 100%
}

[System.Serializable]
public class RandomCustomerSpecial
{
    public SO_Customer so_Customer_Special;
    [Range(0f,1f)] public float customerRate = 1f; // 1f = 100%
}