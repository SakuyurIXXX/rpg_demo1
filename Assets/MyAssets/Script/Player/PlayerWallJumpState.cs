using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    // wallJumpState -> airState
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(-player.lookDirection * player.moveSpeed, player.jumpSpeed);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        /*没有继承基类playerState中的Update方法，问题出在player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        可以看到水平方向速度被一直更新成正常移动速度，这样就无法让角色蹬出去*/

        player.anim.SetFloat("Y Velocity", rb.velocity.y);

        // -> airState
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        // -> wallSlideState // 按相反方向键就不会吸上去
        if (player.IsWallDetected() && xInput == player.lookDirection)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
