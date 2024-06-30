using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAttackState : EnemyState
{
    Enemy_Archer enemy;
    Player player;
    public ArcherAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player;

    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(0, 0);

        // �������>1����battleState
        if (triggerCalled && Vector2.Distance(enemy.transform.position, player.transform.position) > 1)
            stateMachine.ChangeState(enemy.battleState);


        // �������<=1ͣ��
        else if (triggerCalled && Vector2.Distance(enemy.transform.position, player.transform.position) <= 1)
            stateMachine.ChangeState(enemy.idleState);
    }
}
