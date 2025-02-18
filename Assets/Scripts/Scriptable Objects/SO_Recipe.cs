using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Recipe", menuName = "Create new Recipe")]
public class SO_Recipe : ScriptableObject
{
    public RecipeType recipeType;
    public string recipeID;
    public string recipeName;
    public Sprite recipeSprite;

    [Header("========== In Game Object ==========")]
    public GameObject recipePrefab;
    [Header("========== ถ้าเป็น Potion ==========")]
    public ColorPotion colorPotion;
}
public enum RecipeType
{
    Potion, // น้ำยา
    Potion_Topping, // ดอกไม้, ลูกตา , แมงมุม
}