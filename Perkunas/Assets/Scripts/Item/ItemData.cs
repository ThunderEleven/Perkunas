using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Building,
    Consumable,
    Equipable
}

public enum ConsumableType
{
    Health,
    Hunger,
    Stamina
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{

    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType itemType;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ConsumableType ConsumableType;

    [Header("Equip")]
    public GameObject equipPrefab;
}
