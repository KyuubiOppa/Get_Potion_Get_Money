using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertRecipe : MonoBehaviour, IInteractable
{
    public SO_Recipe so_Recipe; 
    public CheckRecipe checkRecipe;

    [Header("Outline")]
    public Color onpointerEnterColor;
    public Color onpointerExitColor;
    private Outline outline;


    void Start()
    {
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        if (so_Recipe != null && so_Recipe.recipePrefab != null && checkRecipe != null)
        {
            // ถ้า recipe เป็นประเภทน้ำยา
            if (so_Recipe.recipeType == RecipeType.Potion)
            {
                // สร้าง Object ใหม่ที่ตำแหน่งของ recipe_Potion_DropPoint
                GameObject reciepeObjectPrefab = Instantiate(so_Recipe.recipePrefab, checkRecipe.recipe_Potion_DropPoint.position, Quaternion.identity);

                // เช็คว่ามี Potion_Pour ไหม
                Potion_Pour potionPour = reciepeObjectPrefab.GetComponent<Potion_Pour>();
                if (potionPour != null)
                {
                    potionPour.colorPotion = so_Recipe.colorPotion; // กำหนดค่าประเภทน้ำยาให้ขวด
                    potionPour.SetBottleColor(); // อัพเดตสีทันที
                }

                // เพิ่มสูตรที่ถูกใส่เข้าไปในหม้อปรุงยา (CheckRecipe)
                checkRecipe.currentRecipes.Add(so_Recipe);
                checkRecipe.currentRecipeObjs.Add(reciepeObjectPrefab);

                Debug.Log("ปล่อย " + so_Recipe.recipeName);
                return;
            }

            // ถ้า recipe เป็นประเภทท็อปปิ้งน้ำยา
            if (so_Recipe.recipeType == RecipeType.Potion_Topping)
            {
                // สร้าง Object ใหม่ที่ตำแหน่งของ recipe_PotionTopping_DropPoint
                GameObject reciepeObjectPrefab = Instantiate(so_Recipe.recipePrefab, checkRecipe.recipe_PotionTopping_DropPoint.position, Quaternion.identity);

                // เพิ่มสูตรที่ถูกใส่เข้าไปในหม้อปรุงยา (CheckRecipe)
                checkRecipe.currentRecipes.Add(so_Recipe);
                checkRecipe.currentRecipeObjs.Add(reciepeObjectPrefab);

                Debug.Log("ปล่อย " + so_Recipe.recipeName);
                return;
            }
        }
        else
        {
            Debug.LogWarning("ไม่สามารถสร้าง Recipe ได้: ข้อมูลไม่ครบถ้วน!");
        }
    }


    public void DisableOutline() 
    {
        outline.OutlineColor = onpointerExitColor;
    }

    public void  EnableOutline() 
    {
        outline.OutlineColor = onpointerEnterColor;
    }
}
