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
            // �������ڻ������±��Ƚ���idle������һ��ʱ����ת��move����������ת���ٽ�idle�ٷ�����move
            if (enemy.IsWallDetected() || !enemy.IsGroundDetected())
                enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }

        // idleState -> attackState
        if (enemy.isPlayerDetected())

            if (enemy.isPlayerDetected().distance < enemy.attackDistance && canAttack())
                stateMachine.ChangeState(enemy.attackState);

    }
    // ������ȴ 
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
