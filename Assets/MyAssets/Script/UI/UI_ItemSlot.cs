using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    protected UI ui;
    public InventoryItem item;


    void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        item = null;

        itemImage.sprite = null;
        itemImage.color = Color.clear;

        itemText.text = "";
    }

    // 左键点击进行装备，并隐藏详细信息
    public virtual void OnPointerDown(PointerEventData eventData)
    {

        if (item == null)
            return;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            InventoryManager.instance.RemoveItem(item.data);
            ui.itemToolTip.HideToolTip();
            return;
        }

        if (item.data.itemtype == ItemType.Equipment)
        {
            InventoryManager.instance.EquipItem(item.data);
            ui.itemToolTip.HideToolTip();
        }

    }

    // 鼠标指针进入显示详细信息
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);

        ui.backpackToolTip.ShowToolTip(item.data);

    }

    // 鼠标指针退出隐藏详细信息
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.itemToolTip.HideToolTip();
        ui.backpackToolTip.HideToolTip();
    }
}
