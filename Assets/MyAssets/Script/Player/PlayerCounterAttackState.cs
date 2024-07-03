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

        // 格挡伤害，防御增加0.2倍
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
            // counterAttack成功 条件：attackCheck里得到敌人的碰撞体（而不是攻击判定。。。）且敌人打开了canBeStunned窗口且玩家在弹反窗口内（stateTimer用来控制弹反窗口）
            if (hit.GetComponent<Enemy>() != null && hit.GetComponent<Enemy>().CanBeStunned() && stateTimer >= 0)
            {
                SuccessfulCounterAttack();

                EnemyStats _target = hit.GetComponent<EnemyStats>();
                hit.GetComponent<Enemy>().isStunned = true;                     // 让怪物进入stunState

                player.stats.DoDamageTo(_target, player.lookDirection);           // 造成伤害
            }

            // 弹鬼手
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
        // 松开防御键或弹反成功
        if (Input.GetKeyUp(KeyCode.Mouse1) || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void SuccessfulCounterAttack()
    {
        playerStats.SetInvincibleTime(.2f);                             // 设置无敌时间
        AudioManager.instance.PlaySFX(7, null);                         // 反击成功音效
        player.anim.SetBool("CounterAttackSuccessful", true);           // 播放动画
        player.SetCounterImpactFX();                                    // counterAttack特效
    }
}
