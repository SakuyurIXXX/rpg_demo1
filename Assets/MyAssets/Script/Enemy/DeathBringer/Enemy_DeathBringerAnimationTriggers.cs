using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringerAnimationTriggers : EnemyAnimationTriggers
{


    private Enemy_DeathBringer deathBringer => GetComponentInParent<Enemy_DeathBringer>();

    private void Relocate() => deathBringer.FindPosition();

    private void MakeInvisible() => deathBringer.fx.MakeTransparent(true);

    private void MakeVisible() => deathBringer.fx.MakeTransparent(false);
    public override void AttackTrigger()
    {
        base.AttackTrigger();
        AttackPauseManager.instance.SetImapctFX(deathBringer.impulseSource, new Vector3(0.4f, 0.3f, 0), deathBringer.lookDirection, 6);
    }

}
