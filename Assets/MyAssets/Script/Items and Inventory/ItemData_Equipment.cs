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


    [Header("������ֵ")]
    public int strength; // +1�˺���+1%����
    public int agility; // +1%���ܣ�+1����
    public int intelligence; // +1���ˣ�+1%ħ���ֿ�
    public int vitality; // +3����

    [Header("������ֵ")]
    public int damage;
    public int critChance;
    public int critPower;  // Ĭ��ֵ:150%

    [Header("������ֵ")]
    public int hp;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("ħ����ֵ")]
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

        AddItemEffectDescription(strength, "����");
        AddItemEffectDescription(agility, "����");
        AddItemEffectDescription(intelligence, "����");
        AddItemEffectDescription(vitality, "����");

        AddItemEffectDescription(damage, "�˺�");
        AddItemEffectDescription(critChance, "������");
        AddItemEffectDescription(critPower, "�����˺�");

        AddItemEffectDescription(hp, "HP");
        AddItemEffectDescription(evasion, "����");
        AddItemEffectDescription(armor, "����");
        AddItemEffectDescription(magicResistance, "����");

        AddItemEffectDescription(fireDamage, "�����˺�");
        AddItemEffectDescription(iceDamage, "�����˺�");
        AddItemEffectDescription(lightingDamage, "�׵��˺�");

        // ��䣬����������ʾ����̳���
        if (effectLength < 4)
        {
            for (int i = 0; i < 4 - effectLength; i++)
            {
                sb.AppendLine();
                sb.Append("");
            }
        }

        // ���װ��˵��
        if (itemDescription.Length > 0)
        {
            sb.AppendLine();
            sb.Append(itemDescription);
        }


        return sb.ToString();
    }

    // װ��Ч��������+ 5 ���ף�
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


