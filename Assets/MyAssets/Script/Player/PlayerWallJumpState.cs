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
        /*û�м̳л���playerState�е�Update�������������player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);
        ���Կ���ˮƽ�����ٶȱ�һֱ���³������ƶ��ٶȣ��������޷��ý�ɫ�ų�ȥ*/

        player.anim.SetFloat("Y Velocity", rb.velocity.y);

        // -> airState
        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        // -> wallSlideState // ���෴������Ͳ�������ȥ
        if (player.IsWallDetected() && xInput == player.lookDirection)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
