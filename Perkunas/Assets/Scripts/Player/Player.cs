using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;

    public BuildingManager buildingManager; // 추가
    public PlayerCondition condition;
    public Equipment equip;

    public ItemData itemData;
    public Action addItem;

    public Transform dropPosition;
    public Transform projectileSpawn;

    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        buildingManager = GetComponent<BuildingManager>(); // 추가
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }
}