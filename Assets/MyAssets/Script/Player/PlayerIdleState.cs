using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    // idleState -> moveState
    public PlayerIdleState(Player _player, PlayerStateMachine _playerStateMachine, string _animBoolName) : base(_player, _playerStateMachine, _animBoolName)
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

    public override void Update()
    {
        base.Update();


        // -> moveState
        if (xInput != 0 && player.IsGroundDetected()) // 暂时不知道要不要加player.IsGroundDetected()这个条件，目前看来不加也行，因为按跳了就进入jumpState了，在结束jumpState（airState)之前都不会进入到moveState
            stateMachine.ChangeState(player.moveState);

    }
}
