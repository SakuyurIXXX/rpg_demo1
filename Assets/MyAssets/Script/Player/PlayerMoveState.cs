using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    // moveState -> idleState
    public PlayerMoveState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlaySFX(4, null);
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX(4);
    }

    public override void Update()
    {
        base.Update();

        // -> idleState
        if (xInput == 0 && player.IsGroundDetected())// 暂时不知道要不要加player.IsGroundDetected()这个条件，目前看来不加也行，因为按跳了就进入jumpState了，在结束jumpState（airState)之前都不会进入到moveState
            stateMachine.ChangeState(player.idleState);
    }
}
