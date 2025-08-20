using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickSlot : UIBase
{
    // 퀵슬롯은 인벤토리의 첫번째 줄의 데이터를 가져올 예정
    public ItemSlot[] quickSlots;
    public GameObject quickSlotWindow;
    public Transform slotPanel;
    public UIInventory inventory;

    private int quickSlotNum = 8;

    private void Start()
    {
        inventory = UIManager.Instance.GetUI<UIInventory>();
        quickSlots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < quickSlots.Length; i++)
        {
            quickSlots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            quickSlots[i].index = i;
            quickSlots[i].quickSlot = this;
            quickSlots[i].Clear();
        }
    }

    public void UpdateQuickSlotsUI()
    {
        for (int i = 0; i < quickSlotNum; i++)
        {
            if (!CheckInventorySlots(i))
            {
                quickSlots[i].item = inventory.slots[i].item;
                quickSlots[i].quantity = inventory.slots[i].quantity;
                quickSlots[i].Set();
            }
            else
            {
                quickSlots[i].Clear();
            }
        }
    }

    private bool CheckInventorySlots(int slotNum)
    {
        if (inventory.slots[slotNum].item == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
