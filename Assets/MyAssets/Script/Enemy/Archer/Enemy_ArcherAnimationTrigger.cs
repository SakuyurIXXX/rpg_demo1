using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_ArcherAnimationTrigger : EnemyAnimationTriggers
{
    Enemy_Archer archer => GetComponentInParent<Enemy_Archer>();

    private void ShootTrigger() => archer.ShootTrigger();
}
