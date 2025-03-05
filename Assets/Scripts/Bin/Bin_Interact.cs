using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin_Interact : MonoBehaviour, IInteractable
{

    public CheckRecipe checkRecipe;
    public OrderServe orderServe;

    [Header("Outline")]
    public Color onpointerEnterColor;
    public Color onpointerExitColor;
    public Outline outline;

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
        checkRecipe.ClearCurrentRecipe();
        orderServe.ClearCurrentOrderServeAll();
        Debug.Log("ทิ้งขยะ");
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
