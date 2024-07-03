using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
    public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    PlayerStats playerStats;
    private int protection;

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("CounterAttackSuccessful", false);

        // ���˺�����������0.2��
        playerStats = player.GetComponent<PlayerStats>();
        protection = (int)(playerStats.armor.GetValue() * 0.2);
        playerStats.armor.AddModifier(protection);
    }

    public override void Exit()
    {
        base.Exit();
        playerStats.armor.RemoveModifier(protection);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();


        player.SetVelocity(0, rb.velocity.y);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            // counterAttack�ɹ� ������attackCheck��õ����˵���ײ�壨�����ǹ����ж����������ҵ��˴���canBeStunned����������ڵ��������ڣ�stateTimer�������Ƶ������ڣ�
            if (hit.GetComponent<Enemy>() != null && hit.GetComponent<Enemy>().CanBeStunned() && stateTimer >= 0)
            {
                SuccessfulCounterAttack();

                EnemyStats _target = hit.GetComponent<EnemyStats>();
                hit.GetComponent<Enemy>().isStunned = true;                     // �ù������stunState

                player.stats.DoDamageTo(_target, player.lookDirection);           // ����˺�
            }

            // ������
            if (hit.GetComponent<CastController>() != null && hit.GetComponent<CastController>().CanBeCounter())
            {
                SuccessfulCounterAttack();
            }

            if (hit.GetComponent<ArrowController>() != null)
            {
                SuccessfulCounterAttack();
                hit.GetComponent<ArrowController>().CounterArrow();
            }

        }
        // �ɿ��������򵯷��ɹ�
        if (Input.GetKeyUp(KeyCode.Mouse1) || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void SuccessfulCounterAttack()
    {
        playerStats.SetInvincibleTime(.2f);                             // �����޵�ʱ��
        AudioManager.instance.PlaySFX(7, null);                         // �����ɹ���Ч
        player.anim.SetBool("CounterAttackSuccessful", true);           // ���Ŷ���
        player.SetCounterImpactFX();                                    // counterAttack��Ч
    }
}
