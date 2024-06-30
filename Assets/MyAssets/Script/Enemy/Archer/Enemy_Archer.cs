using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : Enemy
{
    /// <summary>
    /// 每有一个新的状态需要在这里声明和创建对象
    /// 弓箭手的基本属性和方法写在这里
    /// </summary>
    /// 


    [SerializeField] private GameObject arrow;


    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();

        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Move", this);
        battleState = new ArcherBattleState(this, stateMachine, "Idle", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        deadState = new ArcherDeadState(this, stateMachine, "Dead", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
    }


    public void ShootTrigger()
    {
        GameObject arrowInstance = Instantiate(arrow, attackCheck.position, Quaternion.identity);
        arrowInstance.GetComponent<ArrowController>().SetupArrow(7 * lookDirection, stats.damage.GetValue());
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
