using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemSlot : MonoBehaviour
{
    [SerializeField] public CraftRecipeEditor craftItemData;
    
    public Image icon;
    public TextMeshProUGUI quantityText;

    public UICrafting crafting;
    
    public void InitSlot(CraftRecipeEditor recipe)
    {
        craftItemData = recipe;
        icon.sprite = craftItemData.resultItem.resItem.icon;
        quantityText.text = craftItemData.resultItem.num.ToString();
    }

    public void OnClickIcon()
    {
        crafting.ChangeSelectedRecipeInfo(this);
    }
}
