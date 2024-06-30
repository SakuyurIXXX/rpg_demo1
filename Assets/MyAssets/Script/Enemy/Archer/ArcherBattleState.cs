using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
    Enemy_Archer enemy;
    Transform player;
    int moveDir;
    public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
                // π•ª˜¿‰»¥
                if (canAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
                // battleState->idleState
                if (Vector2.Distance(player.transform.position, enemy.transform.position) <= 1)
                    stateMachine.ChangeState(enemy.idleState);

            }
        }
        else
        {
            // battleState->idleState
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 15)
                stateMachine.ChangeState(enemy.idleState);

        }

        if (player.position.x > enemy.transform.position.x && enemy.lookDirection == -1)  // ÕÊº“‘⁄π÷ŒÔ”“±ﬂ
            enemy.Flip();
        else if (player.position.x < enemy.transform.position.x && enemy.lookDirection == 1)  // ÕÊº“‘⁄π÷ŒÔ◊Û±ﬂ
            enemy.Flip();



    }

    // π•ª˜¿‰»¥ 
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
