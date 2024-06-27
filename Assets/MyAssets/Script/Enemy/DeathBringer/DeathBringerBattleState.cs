using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
    Enemy_DeathBringer enemy;
    Transform player;
    int moveDir;
    public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.isPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            // battleState -> attackState
            if (enemy.isPlayerDetected().distance < enemy.attackDistance)
            {
                enemy.SetVelocity(0, 0);
                // 攻击冷却
                if (canAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
                else
                    stateMachine.ChangeState(enemy.idleState);

            }

            // 状态时间结束->teleport || 玩家站在裆下
            if (stateTimer < 0 && enemy.isPlayerDetected().distance < 5f || enemy.isPlayerDetected().distance < .1)
                stateMachine.ChangeState(enemy.teleportState);

            // 距离玩家过远直接施法
            if (enemy.isPlayerDetected().distance > 13f)
                stateMachine.ChangeState(enemy.spellCastState);
        }


        if (player.position.x > enemy.transform.position.x)// 玩家在怪物右边
            moveDir = 1;
        else if (player.position.x < enemy.transform.position.x)// 玩家在怪物左边
            moveDir = -1;

        if (enemy.isPlayerDetected() && enemy.isPlayerDetected().distance < enemy.attackDistance - .1f) // 防止走进玩家里边，位置和玩家重复，导致一直转向
            return;

        if (Vector2.Distance(player.transform.position, enemy.transform.position) > 1)
            enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);




    }

    // 攻击冷却 
    private bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCoolDown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
