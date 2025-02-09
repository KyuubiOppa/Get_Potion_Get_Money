using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckRecipe_UI : MonoBehaviour
{
    public Button mixButton;
    public Button clearRecipeButton;
    private CheckRecipe checkRecipe;

    void Start()
    {
        checkRecipe = GetComponent<CheckRecipe>();

        mixButton.onClick.AddListener(checkRecipe.MixRecipeToOrder);
        clearRecipeButton.onClick.AddListener(checkRecipe.ClearCurrentRecipe);
    }

    void Update()
    {
        CheckMixButton();
    }

/// <summary>
/// เช็คว่าถ้าตัว CheckRecipe มี Output ที่ถูกให้เปิดปุ่ม Mix ออกมา
/// </summary>
    void CheckMixButton()
    {
        if (checkRecipe.orderOutput == null)
        {
            mixButton.gameObject.SetActive(false);
        }
        else
        {
            mixButton.gameObject.SetActive(true);
        }
    }
}
