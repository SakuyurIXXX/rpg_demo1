using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    // airState -> idleState
    // airState -> wallSlideState
    // airState -> edgeGrabState
    private float coyoteTimer;

    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        coyoteTimer = 0.3f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        coyoteTimer -= Time.deltaTime;

        // -> jumpState
        if (coyoteTimer > 0 && Input.GetKeyDown(KeyCode.Space) && player.isJumped == false)
        {
            stateMachine.ChangeState(player.jumpState);
            player.isJumped = true;
        }

        // -> idleState
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            player.isJumped = false; // 只有从空中落地才会设置 isJumped = false
        }

        // -> wallSlideState
        if (player.IsWallDetected() && player.skills.wallBounce.unlocked == true)
            stateMachine.ChangeState(player.wallSlideState);

        // -> edgeGrabState
        if (!player.IsEdgeDetected() && player.IsWallDetected())
            stateMachine.ChangeState(player.edgeGrabState);

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
