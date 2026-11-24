/*  Item.cs
    Item management.

    Reference:
    Big and Crispy. "Inventory System | Unity Tutorial." YouTube.
    https://www.youtube.com/watch?v=-IPjFSWeyrg

    Adapted for this game project.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Equipment slots
public enum SlotTag { None, Head, Chest, Shield }
public enum ItemType { None, HealthPotion, Equipment, Key }

[CreateAssetMenu(menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public Sprite sprite;
    public SlotTag itemTag;

    [Header("If the item can be equipped")]
    public GameObject equipmentPrefab;

    [Header("Item Type")]
    public ItemType itemType;

    [Header("Potion Effects")]
    public int healthRestore = 5;

    [Header("Shop Prices")]
    public int buyPrice = 30;
    public int sellPrice = 15;

    [Header("Equipment Stats")]
    public int defenseBonus;
    public int attackBonus;
    public int healthBonus;
}
