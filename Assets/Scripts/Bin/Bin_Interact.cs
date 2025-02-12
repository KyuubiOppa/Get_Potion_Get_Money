using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin_Interact : MonoBehaviour, IInteractable
{
    public Outline outline;

    public CheckRecipe checkRecipe;
    public OrderServe orderServe;

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
        orderServe.ClearCurrentOrderServe();
        Debug.Log("ทิ้งขยะ");
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
