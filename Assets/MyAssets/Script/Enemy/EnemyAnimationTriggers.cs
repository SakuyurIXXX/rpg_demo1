using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationTriggers : MonoBehaviour
{
    // ע�����PlayerAnimationTriggers.cs
    Enemy enemy => GetComponentInParent<Enemy>();

    private void AnimationFinishTrigger() => enemy.AnimationFinishTrigger();

    public virtual void AttackTrigger() // ����һ֡����¼������Χ�ڵ���ײ��
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //�����ײ��������ң���������˺��ĺ���
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
