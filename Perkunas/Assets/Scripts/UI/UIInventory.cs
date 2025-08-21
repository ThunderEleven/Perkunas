using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;
using Enumerable = System.Linq.Enumerable;

public class UIInventory : UIBase
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;      // item 버릴 때 필요한 위치

    // 선택한 슬롯의 아이템 정보 표시 위한 UI
    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

				// Action 호출 시 필요한 함수 등록
        controller.inventory += Toggle;      // inventory 키 입력 시
        CharacterManager.Instance.Player.addItem += AddItem;  // 아이템 파밍 시

				// Inventory UI 초기화 로직들
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }

		// 선택한 아이템 표시할 정보창 Clear 함수
		void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

		// Inventory 창 Open/Close 시 호출
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

		
    public void AddItem()
    {
		    // 10강 ItemObject 로직에서 Player에 넘겨준 정보를 가지고 옴
        ItemData data = CharacterManager.Instance.Player.itemData;

				// 여러개 가질 수 있는 아이템이라면
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }

				// 빈 슬롯 찾기
        ItemSlot emptySlot = GetEmptySlot();

				// 빈 슬롯이 있다면
        if(emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
				// 빈 슬롯 마저 없을 때
        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    public void AddItemFromCrafting(ItemData resultItem)
    {
        if (resultItem.canStack)
        {
            ItemSlot slot = GetItemStack(resultItem);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        
        ItemSlot emptySlot = GetEmptySlot();
        if(emptySlot != null)
        {
            emptySlot.item = resultItem;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }
        
        ThrowItem(resultItem);
        CharacterManager.Instance.Player.itemData = null;
    }

    public bool CheckRequireItem(CraftRecipeEditor recipe)
    {
        // 딕션너리로 재료 관리 -> 아이템 이름, 수량
        Dictionary<string, int> curInventoryData = new Dictionary<string, int>();

        foreach (var slot in slots)
        {
            if (slot.item != null)
            {
                string itemName = slot.item.displayName;
                if (curInventoryData.ContainsKey(itemName))
                {
                    curInventoryData[itemName] += slot.quantity;
                }
                else
                {
                    curInventoryData.Add(itemName, slot.quantity);
                }
            }
        }
        
        // 레시피에 필요한 재료 아이템들을 순회하며 인벤토리와 비교
        foreach (var data in recipe.requireItems)
        {
            string requiredItemName = data.reqItem.displayName;
            int requiredNum = data.num;

            // 딕셔너리에 필요한 아이템이 있는지, 충분한 수량이 있는지 확인
            if (curInventoryData.ContainsKey(requiredItemName))
            {
                // 인벤토리의 수량이 필요한 수량보다 적을 경우
                if (curInventoryData[requiredItemName] < requiredNum)
                {
                    Debug.Log($"재료 부족: {requiredItemName}. 필요 수량: {requiredNum}, 현재 수량: {curInventoryData[requiredItemName]}");
                    return false;
                }
            }
            else
            {
                // 필요한 아이템이 인벤토리에 아예 없는 경우
                Debug.Log($"재료 부족: {requiredItemName}. 인벤토리에 존재하지 않습니다.");
                return false;
            }
        }

        // 모든 재료가 충분하다면 true 반환
        return true;
    }

    public void ConsumeItems(CraftRecipeEditor recipe)
    {
        // 딕셔너리를 통해 재료 정보를 관리
        Dictionary<string, int> remainingRequiredItems = new Dictionary<string, int>();
        foreach (var data in recipe.requireItems)
        {
            remainingRequiredItems.Add(data.reqItem.displayName, data.num);
        }

        // 인벤토리 슬롯들을 순회하며 재료를 소모
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];
            // 슬롯에 아이템이 있고, 남은 재료 목록에 해당 아이템이 있다면
            if (slot.item != null && remainingRequiredItems.ContainsKey(slot.item.displayName))
            {
                string itemName = slot.item.displayName;
                int requiredCount = remainingRequiredItems[itemName];

                // 현재 슬롯의 수량이 필요한 수량보다 많거나 같을 경우
                if (slot.quantity >= requiredCount)
                {
                    // 필요한 만큼만 소모
                    slot.quantity -= requiredCount; 
                    // 이 재료는 소모 완료
                    remainingRequiredItems[itemName] = 0; 

                    // 슬롯의 수량이 0이 되면 슬롯을 비움
                    if (slot.quantity == 0)
                    {
                        slot.Clear();
                    }
                }
                // 현재 슬롯의 수량이 필요한 수량보다 적을 경우
                else
                {
                    // 필요한 수량에서 현재 슬롯의 수량만큼 뺀다
                    remainingRequiredItems[itemName] -= slot.quantity;
                    slot.Clear(); 
                }
            }

            // 모든 재료가 소모되었는지 확인하여 루프를 조기 종료 (LinQ 활용)
            if (remainingRequiredItems.All(key => key.Value <= 0))
            {
                Debug.Log("모든 재료를 성공적으로 소모했습니다.");
                break;
            }
        }
    }
    
		// UI 정보 새로고침
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
		        // 슬롯에 아이템 정보가 있다면
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
        
        UIManager.Instance.GetUI<UIQuickSlot>().UpdateQuickSlotsUI();
    }

		// 여러개 가질 수 있는 아이템의 정보 찾아서 return
    ItemSlot GetItemStack(ItemData data)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

		// 슬롯의 item 정보가 비어있는 정보 return
    ItemSlot GetEmptySlot()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

		// Player 스크립트 먼저 수정
		// 아이템 버리기 (실제론 매개변수로 들어온 데이터에 해당하는 아이템 생성)
		public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }


		// ItemSlot 스크립트 먼저 수정
		// 선택한 아이템 정보창에 업데이트 해주는 함수
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for(int i = 0; i< selectedItem.item.consumables.Length; i++)
        {
            selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }
        
        useButton.SetActive(selectedItem.item.itemType == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.itemType == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
         if(selectedItem.item.itemType == ItemType.Consumable)
         {
             for(int i = 0; i < selectedItem.item.consumables.Length; i++)
             {
                 switch (selectedItem.item.consumables[i].type)
                 {
                     case ConsumableType.Health:
                         condition.AddHp(selectedItem.item.consumables[i].value); break;
                     case ConsumableType.Hunger:
                        condition.AddHunger(selectedItem.item.consumables[i].value);break;
                     case ConsumableType.Stamina:
                         condition.AddStamina(selectedItem.item.consumables[i].value); break;
                     case ConsumableType.Thirst:
                         condition.AddThirst(selectedItem.item.consumables[i].value); break;
                }
             }
             RemoveSelctedItem();
         }
     }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelctedItem();
    }

    void RemoveSelctedItem()
    {
        selectedItem.quantity--;
    
        if(selectedItem.quantity <= 0)
        {
            // if (slots[selectedItemIndex].equipped)
            // {
            //     UnEquip(selectedItemIndex);
            // }
    
            selectedItem.item = null;
            ClearSelectedItemWindow();
        }
    
        UpdateUI();
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }
}