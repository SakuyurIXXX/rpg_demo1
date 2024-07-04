using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImages;

    public void SetupCraftWindow(ItemData_Equipment _data)
    {
        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImages.Length; i++)
        {
            materialImages[i].color = Color.clear;
            materialImages[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for (int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if (_data.craftingMaterials.Count > materialImages.Length)
                Debug.LogWarning("Ŀ����ϱȲ��ϸ��Ӷ�");

            materialImages[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImages[i].color = Color.white;

            TextMeshProUGUI materialSlotText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();

            materialSlotText.text = _data.craftingMaterials[i].stackSize.ToString();
            materialSlotText.color = Color.white;
        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.itemName;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => InventoryManager.instance.CanCraft(_data, _data.craftingMaterials));
    }

}
