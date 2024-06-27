using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinIdleState : GoblinGroundedState
{
    public GoblinIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    // idleState -> moveState
    // idleState -> BattleState


    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, rb.velocity.y);

        // idleState -> moveState
        if (stateTimer < 0)
        {
            // 会在碰壁或者悬崖边先进入idle，发呆一段时间再转身move，而不是先转身再进idle再发呆再move
            if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
                enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }

        // idleState -> attackState
        if (enemy.isPlayerDetected())

            if (enemy.isPlayerDetected().distance < enemy.attackDistance && canAttack())
                stateMachine.ChangeState(enemy.attackState);

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
