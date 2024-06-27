using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
    private float lastTimeAttacked;
    private float comboWindow = .5f;

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (player.comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            player.comboCounter = 0;
        player.anim.SetInteger("comboCounter", player.comboCounter);

        switch (player.comboCounter)
        {
            case 0:
                AudioManager.instance.PlaySFX(0, null);
                break;
            case 1:
                AudioManager.instance.PlaySFX(1, null);
                break;
            case 2:
                AudioManager.instance.PlaySFX(2, null);
                break;
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.comboCounter++;
        player.isHit = false;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.SetVelocity(xInput * player.moveSpeed * 0.1f, rb.velocity.y);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);


    }


}
