using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;

/// <summary>
/// ���̳еĻ��࣬���������state����Ҫ�̳���
/// ʵ����Player��û�н�������״̬�����еĵ��þ����ڴ˽ű����ӽű�
/// ��״̬��ص����Ժͷ���д������
/// </summary>
/// 

public class PlayerState
{
    #region Components
    protected PlayerStateMachine stateMachine; // ����PlayerStateMachine�࣬�Գн�����Player.cs�������Ķ���
    protected Player player; // ����Player�࣬�Գн�����Player.cs�������Ķ���
    protected Rigidbody2D rb;//���򻯣���player.rb --> rb
    #endregion

    private string animBoolName;            // ������name��������
    protected float xInput;                 // ��Ϊmove��idle����״̬�������ƶ���������״̬������Զ�����player��Ļ�������
    protected float yInput;

    protected float stateTimer;             // ״̬��ʱ��������״̬����ʱ��
    protected bool triggerCalled;           // �ж��˳�״̬��Ŀǰֻ������Ӧ�����������һ֡���˳���ǰcombo������һcombo��idle״̬

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }//���캯�������ڴ�����Ϣ

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);    // ͨ��animator�Դ��ĺ�������animator���Boolֵ�ı�Ϊ��Ҫ��ֵ
        rb = player.rb;                             // ���򻯣���player.rb --> rb
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

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y); // ������״̬�ж����Ժ����ƶ�

        player.anim.SetFloat("Y Velocity", rb.velocity.y); // ���y���ٶȲ���ֵ����animator���и���"Y Velocity"���ŵĶ���
        stateTimer -= Time.deltaTime; //״̬����ʱ�䵹��ʱ������״̬�ڽ���ʱ����stateTimer������ʱ����ʱ�˳���״̬

    }

    public virtual void AnimationFinishTrigger()
    // ��������뿴PlayerAnimationTriggers.cs
    {
        triggerCalled = true;
    }


}