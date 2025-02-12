using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixRecipeToOrder : MonoBehaviour, IInteractable
{
    public Outline outline;

    public bool canMix = false;

    public CheckRecipe checkRecipe;
    Collider col;

    void Start()
    {
        col = GetComponent<Collider>();
        outline = GetComponent<Outline>();
        DisableOutline();
    }

    void Update()
    {
        if (checkRecipe.orderOutput == null)
        {
            canMix = false;
            col.enabled = false;
        }
        else
        {
            canMix = true;
            col.enabled = true;
        }

        if (canMix)
        {
            EnableOutline();

            // กระพริบ outlineWidth โดยใช้ Mathf.PingPong
            float minWidth = 5f; // ค่าต่ำสุดของ outlineWidth
            float maxWidth = 10f; // ค่าสูงสุดของ outlineWidth
            float speed = 10f;    // ความเร็วของการกระพริบ

            // คำนวณ outlineWidth ให้วิ่งขึ้นลงระหว่าง minWidth และ maxWidth
            float pingPongValue = Mathf.PingPong(Time.time * speed, maxWidth - minWidth);
            outline.OutlineWidth = minWidth + pingPongValue;
        }
        else
        {
            DisableOutline();
        }
    }

    public void Interact()
    {
        if (canMix)
        {
            checkRecipe.MixRecipeToOrder();
            Debug.Log("ผสม");
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