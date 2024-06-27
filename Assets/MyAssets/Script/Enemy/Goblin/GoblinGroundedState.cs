using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinGroundedState : EnemyState
{
    // groundedState -> BattleState
    protected Enemy_Goblin enemy;
    protected Transform player;
    public GoblinGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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


        // groundedState -> BattleState
        if (enemy.isPlayerDetected() && Vector2.Distance(enemy.transform.position, player.position) > 1 || Vector2.Distance(enemy.transform.position, player.position) < 2 && Vector2.Distance(enemy.transform.position, player.position) > 1)
            stateMachine.ChangeState(enemy.battleState);
    }


}
