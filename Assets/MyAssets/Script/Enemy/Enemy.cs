using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    /// <summary>
    /// ����Enemy�Ļ���
    /// ���е��˶����еĻ������Ժͷ���д������
    /// ����״̬ȥ��Ӧ���˽ű����������ʹ�������
    /// </summary>
    /// 

    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("������")]
    public bool isStunned;
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject attackHint;

    [Header("�ƶ�")]
    public float moveSpeed;
    private float defaultMoveSpeed;
    public float idleTime;

    [Header("����")]
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

    // ���ʵ��
    public virtual void DeadTrigger() => Destroy(gameObject);

    // �ŵ�����ǰ̧�ֶ���֡
    public virtual void ReadyToAttack()
    {
        attackHint.SetActive(true);
    }
    //  �ŵ�����ǰһ֡���򿪷�������
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
    }
    // �ŵ�����֡���رշ�������
    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        attackHint.SetActive(false);
    }

    // �ж��ܷ񱻷�������
    public virtual bool CanBeStunned()
    {
        // ����ܣ��رշ������ڣ�������
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        // ���򷵻ؼ�
        return false;
    }


    public virtual RaycastHit2D isPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * lookDirection, 20, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        // ���������ж���
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * lookDirection, transform.position.y));
    }

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
