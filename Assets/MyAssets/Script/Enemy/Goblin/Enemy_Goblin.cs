using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Goblin : Enemy
{
    /// <summary>
    /// ÿ��һ���µ�״̬��Ҫ�����������ʹ�������
    /// �粼�ֵĻ������Ժͷ���д������
    /// </summary>
    #region States
    public GoblinIdleState idleState { get; private set; }
    public GoblinMoveState moveState { get; private set; }
    public GoblinBattleState battleState { get; private set; }
    public GoblinAttackState attackState { get; private set; }
    public GoblinStunnedState stunnedState { get; private set; }
    public GoblinDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new GoblinIdleState(this, stateMachine, "Idle", this);
        moveState = new GoblinMoveState(this, stateMachine, "Move", this);
        battleState = new GoblinBattleState(this, stateMachine, "Move", this);
        attackState = new GoblinAttackState(this, stateMachine, "Attack1", this);
        stunnedState = new GoblinStunnedState(this, stateMachine, "Stunned", this);
        deadState = new GoblinDeadState(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        StunCheck();
    }

    private void StunCheck()
    {
        if (isStunned)
        {
            stateMachine.ChangeState(stunnedState);
            isStunned = false;
        }
    }

    public override bool CanBeStunned()
    {
        return base.CanBeStunned();
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

}
