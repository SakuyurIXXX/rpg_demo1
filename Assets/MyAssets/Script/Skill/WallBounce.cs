using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBounce : Skill
{
    protected override void CheckUnlocked()
    {
        if (skillTreeSlot.unlocked)
            UnlockWallBounce();
    }

    public override void UseSkill()
    {
        base.UseSkill();

    }

    public void UnlockWallBounce()
    {
        unlocked = true;
        skillTreeSlot.UnlockSkillSlot();
    }
}
