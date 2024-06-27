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
        if (xInput == 0 && player.IsGroundDetected())// ��ʱ��֪��Ҫ��Ҫ��player.IsGroundDetected()���������Ŀǰ��������Ҳ�У���Ϊ�����˾ͽ���jumpState�ˣ��ڽ���jumpState��airState)֮ǰ��������뵽moveState
            stateMachine.ChangeState(player.idleState);
    }
}
