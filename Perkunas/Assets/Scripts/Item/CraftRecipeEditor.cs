using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RequireItemInfo
{
    public ItemData reqItem;
    public int num;

    public RequireItemInfo(ItemData itemData, int number)
    {
        this.reqItem = itemData;
        this.num = number;
    }
}

[System.Serializable]
public class ResultItemInfo
{
    public ItemData resItem;
    public int num;
}

[CreateAssetMenu(fileName = "ItemRecipe", menuName = "New ItemRecipe")]
public class CraftRecipeEditor : ScriptableObject
{
    [Header("필요 아이템 정보")]
    public RequireItemInfo[] requireItems;

    [Header("제작 결과물 아이템 정보")] 
    public ResultItemInfo resultItem;

    // [Header("제작 결과물 아이템 아이콘")] 
    // public Sprite resultItemIcon;
}
