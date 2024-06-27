using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerAttackState : EnemyState
{
    Enemy_DeathBringer enemy;
    Player player;

    public DeathBringerAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player;

        enemy.chanceToTeleport += 10;
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




        if (triggerCalled)
        {
            if (enemy.CanTeleport())
                stateMachine.ChangeState(enemy.teleportState);
            else
                stateMachine.ChangeState(enemy.battleState);
        }
    }

    // ¹¥»÷ÀäÈ´ 
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
