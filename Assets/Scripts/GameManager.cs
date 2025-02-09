using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
#region Singleton
    public static GameManager Instance;

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
#endregion

    [Header("________________ Transform ของ Customer ________________")]
    public Transform[] customerTrans; // ตำแหน่งของ Counter ที่ลูกค้าจะไป
    public Transform customerTransStart; // ตำแหน่งที่ลูกค้าจะเริ่มต้น
    public Transform customerTransNull; // ตำแหน่งที่ลูกค้าจะไปเมื่อไม่มี Counter ที่ว่าง
    public Transform customerTransExit; // ตำแหน่งที่ลูกค้าจะไปเมื่อได้รับอาหารครบแล้ว

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
