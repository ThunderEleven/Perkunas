using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{

    public Equip curEquip;
    public GameObject builtObject;
    public Transform equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    private ItemData buildItemData;
    public ItemData BuildItemData { get { return buildItemData; } private set { } }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData data)
    {
        UnEquip();
        if(data.placable)
        {
            builtObject = Instantiate(data.equipPrefab, equipParent);
            buildItemData = data;
        }
        else
        {
            curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
        }

    }

    public void UnEquip()
    {
        if(curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
        if(builtObject != null)
        {
            Destroy(builtObject);
            builtObject = null;
            buildItemData = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
        else if (context.phase == InputActionPhase.Performed && builtObject != null && controller.canLook)
        {
            CharacterManager.Instance.Player.buildingManager.onClick = true;    
        }
    }
}
