using TMPro;
using UnityEngine;

public class UI_StatSlot : MonoBehaviour
{
    [SerializeField] private string statName;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    private void OnValidate()
    {
        gameObject.name = statName;

        if (statNameText != null)
            statNameText.text = statName + ":";
    }
    void Start()
    {
        UpadateStatValueUI();

    }


    public void UpadateStatValueUI()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStats(statType).GetValue().ToString();
        }
    }
}
