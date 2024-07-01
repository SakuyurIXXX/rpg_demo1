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

    public override void Update()
    {
        base.Update();

        // -> airState
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        // -> counterAttackState
        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.counterAttack.unlocked && skills.counterAttack.CanUseSkill() || Input.GetKeyDown(KeyCode.JoystickButton4) && skills.counterAttack.unlocked && skills.counterAttack.CanUseSkill())
            stateMachine.ChangeState(player.counterAttackState);

    }
}
