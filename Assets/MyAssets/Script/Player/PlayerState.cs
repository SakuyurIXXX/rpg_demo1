using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

/// <summary>
/// 被继承的基类，后面的所有state都需要继承它
/// 实际上Player并没有进入过这个状态，所有的调用均属于此脚本的子脚本
/// 和状态相关的属性和方法写在这里
/// </summary>
/// 

public class PlayerState
{
    #region Components
    protected PlayerStateMachine stateMachine; // 创建PlayerStateMachine类，以承接来自Player.cs传过来的东西
    protected Player player; // 创建Player类，以承接来自Player.cs传过来的东西
    protected Rigidbody2D rb;//纯简化，将player.rb --> rb
    #endregion

    private string animBoolName;            // 动画的name，传参用
    protected float xInput;                 // 因为move和idle属于状态，所以移动输入属于状态相关属性而不是player里的基本属性
    protected float yInput;

    protected float stateTimer;             // 状态计时器，控制状态持续时间
    protected bool triggerCalled;           // 判断退出状态，目前只用来对应攻击动画最后一帧，退出当前combo进入下一combo或idle状态

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }//构造函数，用于传递信息

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);    // 通过animator自带的函数，将animator里的Bool值改变为想要的值
        rb = player.rb;                             // 纯简化，将player.rb --> rb
        triggerCalled = false;

    }
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void FixedUpdate()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y); // 在所有状态中都可以横向移动

        player.anim.SetFloat("Y Velocity", rb.velocity.y); // 监测y轴速度并传值，在animator中有根据"Y Velocity"播放的动画
        stateTimer -= Time.deltaTime; //状态持续时间倒计时，部分状态在进入时重置stateTimer，倒计时归零时退出该状态

    }

    public virtual void AnimationFinishTrigger()
    // 具体解释请看PlayerAnimationTriggers.cs
    {
        triggerCalled = true;
    }


}