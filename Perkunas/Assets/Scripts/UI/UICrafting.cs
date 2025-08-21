using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICrafting : UIBase
{
    public GameObject craftingUI;
    public CraftingItemSlot[] slots;
    public Transform slotsPanel;

    [SerializeField] private List<CraftRecipeEditor> recipeList = new List<CraftRecipeEditor>();
    
    private void Start()
    {
        craftingUI.SetActive(false);

        slots = new CraftingItemSlot[slotsPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotsPanel.GetChild(i).GetComponent<CraftingItemSlot>();
            slots[i].InitSlot(recipeList[i]);
        }
    }

    public override void OpenUI()
    {
        craftingUI.SetActive(true);
    }
}
