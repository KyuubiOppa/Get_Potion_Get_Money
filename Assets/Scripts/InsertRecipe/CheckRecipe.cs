using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckRecipe : MonoBehaviour
{
    public Transform recipeDropPoint; // ตำแหน่งปล่อยวัตถุดิบลงมา
    public Transform orderOutputDropPoint; // ตำแหน่งปล่อย Output ออกมา
    [Header("Order ที่สร้างขึ้นหากสูตรถูกต้อง")]
    public SO_Order orderOutput; // Order ที่สร้างขึ้นหากสูตรถูกต้อง

    [Header("สูตรของ Order ที่ถูกต้อง")]
    public SO_Order[] so_Orders; // สูตรของ Order ที่ถูกต้อง
    [Header("วัตถุดิบที่ถูกใส่ในหม้อปรุงยาในปัจจุบัน")]
    public List<SO_Recipe> currentRecipes = new List<SO_Recipe>(); // วัตถุดิบที่ถูกใส่ในหม้อปรุงยาในปัจจุบัน

    public List<GameObject> currentRecipeObjs = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {
        CheckCorrectRecipe();
    }

#region เช็คสูตร
    /// <summary>
    /// เช็คสูตรที่ถูกต้อง
    /// </summary>
    public void CheckCorrectRecipe()
    {
        foreach (SO_Order order in so_Orders) // วนลูปตรวจสอบทุกสูตร Order
        {
            if (IsRecipeMatch(order.recipes)) // ตรวจสอบว่าสูตรตรงกันหรือไม่
            {
                orderOutput = order; // กำหนดสูตรที่ตรงให้เป็น orderOutput
                return; // หยุดการทำงานหลังจากเจอสูตรที่ตรงกัน
            }
        }

        // ถ้าไม่พบสูตรที่ตรงกัน
        orderOutput = null;
        //Debug.LogWarning("ไม่มีสูตรที่ตรงกัน!");
    }

    /// <summary>
    /// ตรวจสอบว่า currentRecipes ตรงกับสูตรที่กำหนดหรือไม่
    /// </summary>
    private bool IsRecipeMatch(SO_Recipe[] targetRecipes)
    {
        // ถ้าจำนวนวัตถุดิบไม่ตรงกับสูตร ให้รีเทิร์น false ทันที
        if (currentRecipes.Count != targetRecipes.Length)
            return false;

        // ใช้ LINQ เช็คว่าทุกวัตถุดิบใน currentRecipes มีอยู่ใน targetRecipes แบบพอดีหรือไม่
        return !targetRecipes.Except(currentRecipes).Any();
    }
#endregion

/// <summary>
/// ล้าง Recipe ที่มีใน currentRecipes และทำลาย GameObject ของวัตถุดิบ
/// </summary>
    public void ClearCurrentRecipe()
    {
        if (currentRecipes.Count > 0 || currentRecipeObjs.Count > 0)
        {
            // ทำลาย GameObject ที่อยู่ใน currentRecipeObjs
            foreach (GameObject obj in currentRecipeObjs)
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }

            // เคลียร์ลิสต์ของ GameObject และ SO_Recipe
            currentRecipeObjs.Clear();
            currentRecipes.Clear();

            Debug.Log("ล้างวัตถุดิบทั้งหมดในหม้อปรุงยาแล้ว!");
        }
        else
        {
            Debug.LogWarning("ไม่มีวัตถุดิบให้ล้าง!");
        }
    }

/// <summary>
/// ผสม Recipe เป็น Order
/// </summary>
    public void MixRecipeToOrder()
    {
        if (orderOutput != null && orderOutput.orderPrefab != null && orderOutputDropPoint != null)
        {
            // สร้าง Order ที่ตำแหน่ง orderOutputDropPoint
            Instantiate(orderOutput.orderPrefab, orderOutputDropPoint.position, Quaternion.identity);

            // ล้างวัตถุดิบทั้งหมด
            ClearCurrentRecipe();

            Debug.Log("สร้าง Order: " + orderOutput.orderName + " และล้างวัตถุดิบทั้งหมด!");
        }
        else
        {
            Debug.LogWarning("ไม่สามารถผสมสูตรได้: ไม่มีสูตรที่ถูกต้องหรือตำแหน่ง Output ไม่ถูกต้อง!");
        }
    }
}
