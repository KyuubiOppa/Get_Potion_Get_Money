using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Customer", menuName = "Create new Customer")]
public class SO_Customer : ScriptableObject
{
    public GameObject customerPrefab;
    [Header("==== Customer Data ====")]
    public string customerID;
    public string customerName;
    public float customerPatience = 60f; // ความอดทนลูกค้า

    [Header("==== Customer Sprite ====")]
    public Sprite customerSmile;
    public Sprite customerTight;
    public Sprite customerAngry;
}
public enum CustomerEmotional
{
    Happy,
    Normal,
    Angry
}