using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    // 状态类，用来控制数值
    [SerializeField] private int baseValue;
    public List<int> modifiers;

    public int GetValue()
    {
        int finalValue = baseValue;
        foreach (int modifier in modifiers)
            finalValue += modifier;
        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    // 装备了武器或者上了什么buff就需要用到modifier去修改数值
    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }
}
