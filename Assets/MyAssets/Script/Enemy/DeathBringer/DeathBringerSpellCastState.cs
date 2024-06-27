using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerSpellCastState : EnemyState
{
    Enemy_DeathBringer enemy;

    private int amountOfSpells;
    private float spellTimer;
    public DeathBringerSpellCastState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        amountOfSpells = enemy.amountOfSpells;
        spellTimer = .5f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.lastTimeCast = Time.time;
    }

    public override void Update()
    {
        base.Update();
        spellTimer -= Time.deltaTime;

        // 能施法就施法
        if (CanCast())
            enemy.CastSpell();

        // 施法次数用完回到battleState
        if (amountOfSpells <= 0)
            stateMachine.ChangeState(enemy.idleState);
    }

    private bool CanCast()
    {
        if (amountOfSpells > 0 && spellTimer < 0)
        {
            amountOfSpells--;
            spellTimer = enemy.spellCoolDown;
            return true;
        }
        return false;
    }
}
