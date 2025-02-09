using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertRecipe : MonoBehaviour, IInteractable
{
    public SO_Recipe so_Recipe; 
    public CheckRecipe checkRecipe;

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
            // สร้าง Object ใหม่ที่ตำแหน่งของ recipeDropPoint
            GameObject reciepeObjectPrefab = Instantiate(so_Recipe.recipePrefab, checkRecipe.recipeDropPoint.position, Quaternion.identity);

            // เพิ่มสูตรที่ถูกใส่เข้าไปในหม้อปรุงยา (CheckRecipe)
            checkRecipe.currentRecipes.Add(so_Recipe);
            checkRecipe.currentRecipeObjs.Add(reciepeObjectPrefab);

            Debug.Log("ปล่อย " + so_Recipe.recipeName);
        }
        else
        {
            Debug.LogWarning("ไม่สามารถสร้าง Recipe ได้: ข้อมูลไม่ครบถ้วน!");
        }
    }

    public void DisableOutline() 
    {
        outline.enabled = false;
    }

    public void  EnableOutline() 
    {
        outline.enabled = true;
    }
}
