using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public BuildingManager buildingManager; // 추가
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        buildingManager = GetComponent<BuildingManager>(); // 추가
    }
}
