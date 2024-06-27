using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
    Enemy_DeathBringer enemy;
    Player player;
    public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;

        player = PlayerManager.instance.player;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(0, rb.velocity.y);

        if (Vector2.Distance(player.transform.position, enemy.transform.position) < 15 && enemy.isPlayerDetected())
            enemy.bossFightBegun = true;

        if (stateTimer < 0 && enemy.bossFightBegun == true)
            stateMachine.ChangeState(enemy.battleState);

    }

}
