using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterSkill : Skill
{

    protected override void CheckUnlocked()
    {
        if (skillTreeSlot.unlocked)
            UnlockCounter();
    }
    public override void UseSkill()
    {
        base.UseSkill();

    }

    public void UnlockCounter()
    {
        unlocked = true;
        skillTreeSlot.UnlockSkillSlot();
    }
}
