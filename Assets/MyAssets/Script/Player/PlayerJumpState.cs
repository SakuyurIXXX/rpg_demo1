using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    // jumpState -> airState
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.isJumped = true;
        player.SetVelocity(rb.velocity.x, player.jumpSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // -> airState
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        // -> counterAttackState
        if (Input.GetKeyDown(KeyCode.Mouse1) && player.skills.counterAttack.unlocked && player.skills.counterAttack.CanUseSkill() || Input.GetKeyDown(KeyCode.JoystickButton4) && player.skills.counterAttack.unlocked && player.skills.counterAttack.CanUseSkill())
            stateMachine.ChangeState(player.counterAttackState);

        // ->attack
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            player.comboCounter = 2;
            stateMachine.ChangeState(player.primaryAttackState);
        }

    }
}
