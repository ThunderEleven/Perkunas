using System;
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
    Stamina,
    Thirst
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
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
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}
