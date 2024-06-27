using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    /// <summary>
    /// 被所有敌人的所有状态继承的基类
    /// 与状态相关的属性和方法写在这里
    /// </summary>
    /// 

    #region Components
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;
    #endregion

    private string animBoolName;
    protected float stateTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        triggerCalled = true;
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void AnimationFinishTrigger() => triggerCalled = true;

}
