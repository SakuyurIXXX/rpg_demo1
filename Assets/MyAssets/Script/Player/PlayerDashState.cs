using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    // dashState -> idleState
    // dashState -> wallJumpState

    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.stats.MakeInvincible(true);
        // ״̬ʱ�� = ���ʱ��
        stateTimer = player.dashDuration;
        AttackPauseManager.instance.SetImapctFX(player.impulseSource, new Vector3(.3f, 0, 0), player.lookDirection, 0);
    }

    public override void Exit()
    {
        base.Exit();
        player.stats.MakeInvincible(false);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // ɲ�� ������4/7�ڼ��٣�3/7��ɲ�������Լ�ʱ��С��3/7player.dushDurationʱ����
        if (stateTimer < .15f)

            player.SetVelocity(player.lookDirection * player.moveSpeed, 0);
        else
            player.SetVelocity(player.lookDirection * player.moveSpeed * 2.8f, 0);

        // -> idleState
        if (stateTimer < 0)
            stateMachine.ChangeState(player.idleState);


        // -> wallSlideState
        if (!player.IsGroundDetected() && player.IsWallDetected() && skills.wallBounce.unlocked)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
