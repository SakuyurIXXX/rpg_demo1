using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    /// <summary>
    /// 所有Enemy的基类
    /// 所有敌人都具有的基本属性和方法写在这里
    /// 敌人状态去对应敌人脚本那里声明和创建对象
    /// </summary>
    /// 

    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("被击晕")]
    public bool isStunned;
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject attackHint;

    [Header("移动")]
    public float moveSpeed;
    private float defaultMoveSpeed;
    public float idleTime;

    [Header("攻击")]
    public float attackDistance;
    public float attackCoolDown;
    [HideInInspector] public float lastTimeAttacked;
    public float battleTime;

    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed *= 1 - _slowPercentage;
        anim.speed *= 1 - _slowPercentage;

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void AssignLastAnimName(string _animBoolName) => lastAnimBoolName = _animBoolName;

    // 清除实体
    public virtual void DeadTrigger() => Destroy(gameObject);

    // 放到攻击前抬手动作帧
    public virtual void ReadyToAttack()
    {
        attackHint.SetActive(true);
    }
    //  放到攻击前一帧，打开反击窗口
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
    }
    // 放到攻击帧，关闭反击窗口
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        attackHint.SetActive(false);
    }

    // 判断能否被反击击退
    public virtual bool CanBeStunned()
    {
        // 如果能，关闭反击窗口，返回真
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        // 否则返回假
        return false;
    }


    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * lookDirection, 20, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // 攻击距离判定线
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * lookDirection, transform.position.y));
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
