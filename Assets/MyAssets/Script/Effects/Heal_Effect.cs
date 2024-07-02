using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Item Effect/Heal Effect")]
public class Heal_Effect : ItemEffect
{
    [Range(0, 1)]
    [SerializeField] private float healthPercent;

    public override void ExecuteEffect()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        int healValue = Mathf.RoundToInt(playerStats.GetMaxHpValue() * healthPercent);

        playerStats.IncreaseHpBy(healValue);
    }
}
