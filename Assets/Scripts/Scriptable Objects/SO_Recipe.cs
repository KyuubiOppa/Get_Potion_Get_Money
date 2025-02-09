using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Recipe", menuName = "Create new Recipe")]
public class SO_Recipe : ScriptableObject
{
    public string recipeID;
    public string recipeName;
    public Sprite recipeSprite;

    [Header("========== In Game Object ==========")]
    public GameObject recipePrefab;
}
