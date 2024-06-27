using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinStunnedState : EnemyState
{
    // stunnedState -> idleState
    Enemy_Goblin enemy;
    public GoblinStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Goblin _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunDuration;

        enemy.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        rb.velocity = new Vector2(-enemy.lookDirection * enemy.stunDirection.x, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorChange", 0);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
        if (enemy.stats.currentHp <= 0)
            stateMachine.ChangeState(enemy.deadState);
    }
}

