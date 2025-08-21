using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequireItemSlot : MonoBehaviour
{
    [SerializeField] public CraftRecipeEditor craftItemData;
    
    public Image icon;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI quantityText;

    public UICrafting crafting;

    public void InitRequireItemSlot(RequireItemInfo itemInfo)
    {
        icon.sprite = itemInfo.reqItem.icon;
        itemNameText.text = itemInfo.reqItem.displayName;
        quantityText.text = "X" + itemInfo.num.ToString();
    }
}
