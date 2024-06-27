using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BackpackToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    public void ShowToolTip(ItemData item)
    {
        if (item == null)
            return;


        itemNameText.text = item.itemName.ToString();
        itemDescription.text = item.GetDescription();

        AdjustPosition();
        gameObject.SetActive(true);
    }

    public void HideToolTip() => gameObject.SetActive(false);
}
