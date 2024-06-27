using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType equipmentType;

    private void OnValidate()
    {
        gameObject.name = "×°±¸²Û - " + equipmentType.ToString();
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (item == null || item.data == null)
            return;
        InventoryManager.instance.UnequipItem(item.data as ItemData_Equipment);
        InventoryManager.instance.AddItem(item.data as ItemData_Equipment);
        ui.itemToolTip.HideToolTip();
        CleanUpSlot();
    }
}
