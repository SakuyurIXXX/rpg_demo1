using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GoblinAnimationTriggers : EnemyAnimationTriggers
{
    Enemy_Goblin goblin => GetComponentInParent<Enemy_Goblin>();

    private void AnimationFinishTrigger() => goblin.AnimationFinishTrigger();

    public override void AttackTrigger() // 存在一帧，记录攻击范围内的碰撞体
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(goblin.attackCheck.position, goblin.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //如果碰撞体内有玩家，触发造成伤害的函数
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();

                goblin.stats.DoDamageTo(_target, goblin.lookDirection);

                AttackPauseManager.instance.HitPause(goblin.hitPause); // 顿帧


            }
        }
    }


}
