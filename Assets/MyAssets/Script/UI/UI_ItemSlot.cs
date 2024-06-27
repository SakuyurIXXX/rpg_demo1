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

    // ����������װ������������ϸ��Ϣ
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

    // ���ָ�������ʾ��ϸ��Ϣ
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemToolTip.ShowToolTip(item.data as ItemData_Equipment);

        ui.backpackToolTip.ShowToolTip(item.data);

    }

    // ���ָ���˳�������ϸ��Ϣ
    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;
        ui.itemToolTip.HideToolTip();
        ui.backpackToolTip.HideToolTip();
    }
}
