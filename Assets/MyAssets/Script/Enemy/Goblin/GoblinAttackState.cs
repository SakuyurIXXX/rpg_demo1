using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAttackState : EnemyState
{
    Enemy_Goblin enemy;
    Player player;
    public GoblinAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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

        // 距离玩家>1进入battleState
        if (triggerCalled && Vector2.Distance(enemy.transform.position, player.transform.position) > 1)
            stateMachine.ChangeState(enemy.battleState);


        // 距离玩家<=1停下
        else if (triggerCalled && Vector2.Distance(enemy.transform.position, player.transform.position) <= 1)
            stateMachine.ChangeState(enemy.idleState);
    }
}
