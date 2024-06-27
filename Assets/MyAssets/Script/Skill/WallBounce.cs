using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : Skill
{
    protected override void CheckUnlocked()
    {
        if (skillTreeSlot.unlocked)
            UnlockDash();
    }

    public override void UseSkill()
    {
        base.UseSkill();

    }

    public void UnlockDash()
    {
        unlocked = true;
        skillTreeSlot.UnlockSkillSlot();
    }
}
