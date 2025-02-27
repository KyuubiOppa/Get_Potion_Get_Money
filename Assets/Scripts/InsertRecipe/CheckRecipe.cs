using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckRecipe : MonoBehaviour
{
    public Transform recipe_Potion_DropPoint; // ตำแหน่งปล่อยวัตถุดิบประเภทน้ำยา
    public Transform recipe_PotionTopping_DropPoint; // ตำแหน่งปล่อยวัตถุดิบประเภทท็อปปิ้งน้ำยา
    [Header("Order ที่สร้างขึ้นหากสูตรถูกต้อง")]
    public SO_Order orderOutput; // Order ที่สร้างขึ้นหากสูตรถูกต้อง

    [Header("สูตรของ Order ที่ถูกต้อง")]
    public SO_Order[] so_Orders; // สูตรของ Order ที่ถูกต้อง
    [Header("วัตถุดิบที่ถูกใส่ในหม้อปรุงยาในปัจจุบัน")]
    public List<SO_Recipe> currentRecipes = new List<SO_Recipe>(); // วัตถุดิบที่ถูกใส่ในหม้อปรุงยาในปัจจุบัน
    public List<GameObject> currentRecipeObjs = new List<GameObject>();


    public OrderServe orderServe;

    public MeshRenderer waterInWitchPot;

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
                ParticleEffectManager.Instance.ChangeWaterColor(orderOutput.colorPotion, waterInWitchPot);
                return; // หยุดการทำงานหลังจากเจอสูตรที่ตรงกัน
            }
        }

        // ถ้าไม่พบสูตรที่ตรงกัน
        orderOutput = null;
        UpdateWaterColorBasedOnIngredients();

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
        if (orderOutput != null && orderOutput.orderPrefab != null && orderServe.orderOutputDropPoints.Length > 0)
        {
            // หาตำแหน่งปล่อย Order ที่ว่างอยู่
            Transform selectedDropPoint = null;
            foreach (Transform dropPoint in orderServe.orderOutputDropPoints)
            {
                // เช็คว่าตำแหน่งปล่อย Order นั้นว่างหรือไม่ (ไม่มีลูกอยู่)
                if (dropPoint.childCount == 0)
                {
                    selectedDropPoint = dropPoint;
                    break;
                }
            }

            if (selectedDropPoint != null)
            {
                // สร้าง Order Prefab และกำหนดให้เป็นลูกของ selectedDropPoint
                GameObject newOrder = Instantiate(orderOutput.orderPrefab, selectedDropPoint.position, Quaternion.identity, selectedDropPoint);

                // ดึงคอมโพเนนต์ Order_Object จาก GameObject ที่สร้างขึ้น
                Order_Object orderObject = newOrder.GetComponent<Order_Object>();

                if (orderObject != null)
                {
                    // เพิ่ม orderOutput ไปยัง orderInCounters ของ OrderServe
                    orderServe.orderInCounters.Add(orderOutput);

                    // เพิ่ม Order_Object ไปยัง currentOrderObjs ของ OrderServe
                    orderServe.AddOrderObjectInTable();

                    // ล้างวัตถุดิบทั้งหมด
                    ClearCurrentRecipe();

                    Debug.Log("สร้าง Order: " + orderOutput.orderName + " และล้างวัตถุดิบทั้งหมด!");
                }
                else
                {
                    Debug.LogWarning("ไม่พบคอมโพเนนต์ Order_Object ใน Order Prefab!");
                    Destroy(newOrder); // ทำลาย GameObject หากไม่มี Order_Object
                }
            }
            else
            {
                Debug.LogWarning("ไม่สามารถผสมสูตรได้: ตำแหน่งปล่อย Order เต็ม!");
            }
        }
        else
        {
            Debug.LogWarning("ไม่สามารถผสมสูตรได้: ไม่มีสูตรที่ถูกต้องหรือตำแหน่ง Output ไม่ถูกต้อง!");
        }
    }

    /// <summary>
    /// อัพเดตสีน้ำในหม้อตามวัตถุดิบที่ถูกใส่ลงไป
    /// </summary>
    private void UpdateWaterColorBasedOnIngredients()
    {
        // ตรวจสอบว่ามีวัตถุดิบประเภทน้ำยาในหม้อหรือไม่
        List<SO_Recipe> potionRecipes = currentRecipes.Where(recipe => recipe.recipeType == RecipeType.Potion).ToList();

        if (potionRecipes.Count == 0)
        {
            // ถ้าไม่มีวัตถุดิบประเภทน้ำยา ให้ตั้งสีน้ำเป็น สีดำ
            ParticleEffectManager.Instance.ChangeWaterColor(ColorPotion.black, waterInWitchPot);
            return;
        }

        // ตรวจสอบว่ามีการผสมสีแดงและน้ำเงินหรือไม่
        bool hasRed = potionRecipes.Any(recipe => recipe.colorPotion == ColorPotion.red);
        bool hasBlue = potionRecipes.Any(recipe => recipe.colorPotion == ColorPotion.blue);

        // ถ้ามีสีแดงและน้ำเงิน และมีสีอื่นเพิ่มเข้ามา ให้เปลี่ยนสีน้ำเป็นสีดำ
        if (hasRed && hasBlue && potionRecipes.Count > 2)
        {
            ParticleEffectManager.Instance.ChangeWaterColor(ColorPotion.black, waterInWitchPot);
            return;
        }

        // ถ้ามีเพียงสีเดียว ให้ใช้สีนั้น
        if (potionRecipes.Count == 1)
        {
            ParticleEffectManager.Instance.ChangeWaterColor(potionRecipes[0].colorPotion, waterInWitchPot);
            return;
        }

        // ถ้ามีการผสมสีอื่นที่ไม่ใช่แดงและน้ำเงิน ให้เปลี่ยนสีน้ำเป็นดำ
        if (potionRecipes.Count > 1)
        {
            ParticleEffectManager.Instance.ChangeWaterColor(ColorPotion.black, waterInWitchPot);
        }
    }
}
