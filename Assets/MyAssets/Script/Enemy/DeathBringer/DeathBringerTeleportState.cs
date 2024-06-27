using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerTeleportState : EnemyState
{
    Enemy_DeathBringer enemy;
    public DeathBringerTeleportState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.stats.MakeInvincible(true); // 无敌

    }

    public override void Exit()
    {
        base.Exit();


        enemy.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        // teleport结束以后，如果能施法就施法，不能就进入battleState
        if (triggerCalled)
        {
            if (enemy.CanDoSpellCast())
                stateMachine.ChangeState(enemy.spellCastState);
            else
                stateMachine.ChangeState(enemy.battleState);
        }

    }
}
