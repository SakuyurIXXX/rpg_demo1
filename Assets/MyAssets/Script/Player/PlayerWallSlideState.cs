using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    // wallSlideState -> wallJumpState
    // wallSlideState -> idleState
    // wallSlideState -> airState
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void FixedUpdate()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        // -> wallJumpState
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        // 按↓加速掉落的效果
        if (yInput < 0)
            player.SetVelocity(0, rb.velocity.y);

        else
        {
            player.SetVelocity(0, rb.velocity.y * .75f);
        }

        // -> idleState // 落地
        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        // -> airState
        if (!player.IsWallDetected())
            stateMachine.ChangeState(player.airState);
    }
}
