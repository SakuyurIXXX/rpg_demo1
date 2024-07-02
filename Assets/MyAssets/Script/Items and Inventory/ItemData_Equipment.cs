using UnityEngine;

public enum EquipmentType
{
    Weapen,
    Armor,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;

    public float itemCooldown;
    public ItemEffect[] itemEffects;


    [Header("属性数值")]
    public int strength; // +1伤害，+1%爆伤
    public int agility; // +1%闪避，+1暴击
    public int intelligence; // +1法伤，+1%魔法抵抗
    public int vitality; // +3生命

    [Header("攻击数值")]
    public int damage;
    public int critChance;
    public int critPower;  // 默认值:150%

    [Header("防御数值")]
    public int hp;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("魔法数值")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;


    private int effectLength;

    public void ExecuteItemEffect()
    {
        foreach (var item in itemEffects)
            item.ExecuteEffect();
    }

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.hp.AddModifier(hp);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);


    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.hp.RemoveModifier(hp);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public override string GetDescription()
    {
        sb.Length = 0;
        effectLength = 0;

        AddItemEffectDescription(strength, "力量");
        AddItemEffectDescription(agility, "敏捷");
        AddItemEffectDescription(intelligence, "智力");
        AddItemEffectDescription(vitality, "体力");

        AddItemEffectDescription(damage, "伤害");
        AddItemEffectDescription(critChance, "暴击率");
        AddItemEffectDescription(critPower, "暴击伤害");

        AddItemEffectDescription(hp, "HP");
        AddItemEffectDescription(evasion, "闪避");
        AddItemEffectDescription(armor, "护甲");
        AddItemEffectDescription(magicResistance, "法抗");

        AddItemEffectDescription(fireDamage, "火焰伤害");
        AddItemEffectDescription(iceDamage, "冰冻伤害");
        AddItemEffectDescription(lightingDamage, "雷电伤害");

        // 填充，用于设置提示框最短长度
        if (effectLength < 4)
        {
            for (int i = 0; i < 4 - effectLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        // 添加装备说明
        if (itemDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemDescription);
        }


        return sb.ToString();
    }

    // 装备效果描述（+ 5 护甲）
    private void AddItemEffectDescription(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            if (_value > 0)
                sb.Append("+ " + _value + " " + _name);

            effectLength++;
        }

    }
}


