using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMoveState : GoblinGroundedState
{
    public GoblinMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }

    // moveState -> IdleState


    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.lookDirection, rb.velocity.y);

        // moveState -> IdleState
        if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
            stateMachine.ChangeState(enemy.idleState);

    }
}
