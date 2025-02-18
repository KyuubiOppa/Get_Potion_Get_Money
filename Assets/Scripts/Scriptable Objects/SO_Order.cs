using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Order", menuName = "Create new Order")]
public class SO_Order : ScriptableObject
{
    public OrderType orderType; // ประเภทของ Order
    [Header("==== Order Information ====")]
    public string orderID;
    public string orderName;
    public int orderPrice;
    public Sprite orderSprite;
    public GameObject orderPrefab;

    [Header("==== Order Potion Recipe ====")]
    public SO_Recipe[] recipes;

    [Header("==== สีของ Order น้ำยา ====")]
    public ColorPotion colorPotion;
}
public enum OrderType
{
    Potion,

}
public enum ColorPotion
{
    NoColor,
    red,
    yellow,
    blue,
    purple,
    green,
    orange,
    brown,
    black
}