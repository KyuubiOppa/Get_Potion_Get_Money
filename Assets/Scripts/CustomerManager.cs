using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

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

    /// </summary>
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
            Debug.Log("สุ่ม customerRate ของ " + customer.so_Customer.name + " เป็น " + customer.customerRate);
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
            Debug.Log("สุ่ม customerRate ของ " + customerSpecial.so_Customer_Special.name + " เป็น " + customerSpecial.customerRate);
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