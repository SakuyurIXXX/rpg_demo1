using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GoblinAnimationTriggers : EnemyAnimationTriggers
{
    Enemy_Goblin goblin => GetComponentInParent<Enemy_Goblin>();

    private void AnimationFinishTrigger() => goblin.AnimationFinishTrigger();

    public override void AttackTrigger() // ����һ֡����¼������Χ�ڵ���ײ��
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(goblin.attackCheck.position, goblin.attackCheckRadius);

        foreach (var hit in colliders)
        {
            //�����ײ��������ң���������˺��ĺ���
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();

                goblin.stats.DoDamageTo(_target, goblin.lookDirection);

                AttackPauseManager.instance.HitPause(goblin.hitPause); // ��֡


            }
        }
    }


}
