using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICrafting : UIBase
{
    [Header("제작 UI 내부의 오브젝트 관련")]
    public GameObject craftingUI;
    public Transform slotsPanel;
    public Image selectedRecipeIcon;
    public TextMeshProUGUI selectedRecipeName;
    public GameObject requireItemPanel;

    [Header("레시피 슬롯 관련")]
    [SerializeField] private List<CraftRecipeEditor> recipeList = new List<CraftRecipeEditor>();
    public CraftingItemSlot[] slots;

    [Header("필요한 프리팹들")]
    public GameObject requireItemSlotPrefab;
    public GameObject slotPrefab;
    
    private void Start()
    {
        craftingUI.SetActive(false);

        slots = new CraftingItemSlot[recipeList.Count];
        
        CreateSlots();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var prefab = Instantiate(slotPrefab);
            prefab.transform.parent = slotsPanel;
            
            slots[i] = slotsPanel.GetChild(i).GetComponent<CraftingItemSlot>();
            slots[i].InitSlot(recipeList[i]);
            slots[i].crafting = this;
        }
    }

    // 제작 UI의 슬롯을 누르면 제작 UI의 정보가 바뀌는 메서드
    public void ChangeSelectedRecipeInfo(CraftingItemSlot slotInfo)
    {
        selectedRecipeIcon.sprite = slotInfo.icon.sprite;
        selectedRecipeName.text = slotInfo.craftItemData.resultItem.resItem.displayName;

        // 선택한 아이템이 바뀔때마다 기존 정보를 지워야함
        if (requireItemPanel.transform.childCount > 0)
        {
            for (int i = requireItemPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(requireItemPanel.transform.GetChild(i).gameObject);
            }
        }
        
        var requireItemType = slotInfo.craftItemData.requireItems.Length;
        for (int i = 0; i < requireItemType; i++)
        {
            var prefab = Instantiate(requireItemSlotPrefab);
            prefab.GetComponent<RequireItemSlot>().InitRequireItemSlot(slotInfo.craftItemData.requireItems[i]);
            prefab.transform.parent = requireItemPanel.transform;
        }
    }
    
    public override void OpenUI()
    {
        craftingUI.SetActive(true);
    }
}
