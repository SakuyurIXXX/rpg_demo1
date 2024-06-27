using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    // 注释详见PlayerAnimationTriggers.cs
    Enemy enemy => GetComponentInParent<Enemy>();

    private void AnimationFinishTrigger() => enemy.AnimationFinishTrigger();

    public virtual void AttackTrigger() // 存在一帧，记录攻击范围内的碰撞体
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //如果碰撞体内有玩家，触发造成伤害的函数
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();

                enemy.stats.DoDamage(_target, enemy.lookDirection);
            }
        }
    }

    protected virtual void ReadyToAttack() => enemy.ReadyToAttack();
    protected virtual void OpenCounterAttackWindow() => enemy.OpenCounterAttackWindow();
    protected virtual void CloseCounterAttackWindow() => enemy.CloseCounterAttackWindow();

    protected virtual void DeadTrigger() => enemy.DeadTrigger();
}
