using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer_Movement : MonoBehaviour
{
    public float moveSpeed = 2f;

    public Transform target; // ตำแหน่งของ Counter ที่ลูกค้าจะไป

    public List<Customer_Movement> customersInQueue = new List<Customer_Movement>(); // ลูกค้าที่อยู่ในคิวเอาไว้เช็คว่าลูกค้าคนอื่นมีคิวเดียวกันหรือไม่

    void Start()
    {
        
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        if (target == null)
        {
            target = GameManager.Instance.customerTransNull;
        }

        
    }
}
