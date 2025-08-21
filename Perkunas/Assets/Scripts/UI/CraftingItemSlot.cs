using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingItemSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;
    
    [SerializeField] private CraftRecipeEditor craftItemData;

    public void InitSlot(CraftRecipeEditor recipe)
    {
        craftItemData = recipe;
        icon.sprite = craftItemData.resultItemIcon;
        quantityText.text = craftItemData.resultItem.num.ToString();
    }
}
