using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    // GroundedState = idleState + moveState
    // GroundedState -> airState
    // GroundedState -> jumpState
    // GroundedState -> primaryAttackState
    // GroundedState -> counterAttackState
    private SkillManager skills;

    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        skills = SkillManager.instance;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();


        // -> counterAttackState
        if (Input.GetKeyDown(KeyCode.Mouse1) && skills.counterAttack.unlocked && skills.counterAttack.CanUseSkill() || Input.GetKeyDown(KeyCode.JoystickButton4) && skills.counterAttack.unlocked && skills.counterAttack.CanUseSkill())
            stateMachine.ChangeState(player.counterAttackState);

        // -> airState
        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        // -> jumpState
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected() || Input.GetKeyDown(KeyCode.JoystickButton1) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);

        // -> attack
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.JoystickButton5))
        {

            stateMachine.ChangeState(player.primaryAttackState);
        }

    }
}
