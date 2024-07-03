using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEdgeGrabState : PlayerState
{
    // edgeGrabState -> jumpState
    // edgeGrabState -> wallSlideState
    public PlayerEdgeGrabState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = 4;
    }

    public override void FixedUpdate()
    {
        player.SetVelocity(0, 0);

        // ->JumpState
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            stateMachine.ChangeState(player.jumpState);
            return;
        }

        // -> wallSlideState
        if (yInput < 0 && player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);

        // ->airState
        if (Input.GetKeyDown(KeyCode.S))
            stateMachine.ChangeState(player.airState);
    }
}
