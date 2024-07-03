using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : Entity
{
    /// <summary>
    /// ÿ��һ���µ�״̬��Ҫ�����������ʹ�������
    /// ��ҵĻ������Ժͷ���д������
    /// </summary>
    /// 

    //public bool hasAttackInputBuffer;                                           // ����Ԥ�����ж�
    //[SerializeField] private float attackInputBufferTime = 0.2f;                // Ԥ����ʱ��
    //WaitForSeconds waitAttackInputBufferTime;
    [Header("����")]
    public SkillManager skills;
    public Transform interaction_UI;
    [Space]
    public float counterAttackDuration;                                        // ��������ʱ��

    [HideInInspector] public int comboCounter;
    [HideInInspector] public bool isJumped;                                    // Coyote Time���
    //[HideInInspector] public bool isHit;                                       // ��ֹһ�ι�����ɶ���˺�



    #region ��ײ������
    [Header("��ײ���")]

    // ���ڼ��ɻ�����Ʒ

    [SerializeField] protected Transform interactionCheck;
    [SerializeField] protected float interactionCheckDistance;

    // ���ڽ�ɫ��Եץȡ
    [SerializeField] protected Transform edgeCheck;
    [SerializeField] protected float edgeCheckDistance;
    protected bool isEdge;
    #endregion  

    #region �ƶ����
    [Header("�ƶ�")]
    public float moveSpeed;
    public float jumpSpeed;
    public float dashDuration;
    private float defaultMoveSpeed;
    private float defaultJumpSpeed;
    #endregion

    #region ��������
    [Header("��������")]
    private Animator hitAnim;
    private Animator counterAnim;
    public CinemachineImpulseSource impulseSource;
    #endregion  


    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerEdgeGrabState edgeGrabState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerDeadState deadState { get; private set; }
    public PlayerPrimaryAttack primaryAttackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        edgeGrabState = new PlayerEdgeGrabState(this, stateMachine, "EdgeGrab");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        deadState = new PlayerDeadState(this, stateMachine, "Dead");
        primaryAttackState = new PlayerPrimaryAttack(this, stateMachine, "Attack");


        //waitAttackInputBufferTime = new WaitForSeconds(attackInputBufferTime);
    }
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpSpeed = jumpSpeed;

        hitAnim = anim.transform.GetChild(0).GetComponent<Animator>();
        counterAnim = anim.transform.GetChild(1).GetComponent<Animator>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        skills = SkillManager.instance;
    }
    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();
        stateMachine.currentState.FixedUpdate();
        CheckForDash();
        CheckForChest();
        CheckForHealing();

    }

    private void CheckForHealing()
    {
        if ((float)stats.currentHp / stats.GetMaxHpValue() < 0.3)
        {
            ShowInteractionHintUI("F");
            Invoke("CloseInteractionHintUI", 3f);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            InventoryManager.instance.UseFlask();
        }
    }

    private void CheckForChest()
    {
        RaycastHit2D hit = Physics2D.Raycast(interactionCheck.position, Vector2.right * lookDirection, interactionCheckDistance, LayerMask.GetMask("Chest"));
        if (hit.collider != null)
        {
            Chest chest = hit.collider.GetComponent<Chest>();

            ShowInteractionHintUI("E");

            if (Input.GetKeyDown(KeyCode.E))
                chest.GenerateDrop();
        }
        else
            CloseInteractionHintUI();
    }

    private void ShowInteractionHintUI(String text)
    {
        interaction_UI.gameObject.SetActive(true);
        interaction_UI.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
    private void CloseInteractionHintUI()
    {
        interaction_UI.gameObject.SetActive(false);
    }

    //public void CheckAttackBuffer()
    //{

    //}

    public void SetCounterImpactFX()
    {
        counterAnim.SetTrigger("Counter"); // �����Ч

        AttackPauseManager.instance.SetImapctFX(impulseSource, new Vector3(0.05f, 0.05f, 0), lookDirection, 6);
    }



    //������ײ��� ��϶�������AttackAreaʹ��
    private void OnTriggerEnter2D(Collider2D enemyCollision)
    {
        if (enemyCollision.CompareTag("Enemy"))
        {
            EnemyStats _target = enemyCollision.GetComponent<EnemyStats>();

            stats.DoDamageTo(_target, lookDirection);

            if (InventoryManager.instance.GetEquipment(EquipmentType.Weapen) != null)
                InventoryManager.instance.GetEquipment(EquipmentType.Weapen).ExecuteItemEffect();  // ������������Ч������Ѫ����Ч��


            //hitAnim.SetTrigger("Hit"); // �����Ч
            if (comboCounter == 2)
                AttackPauseManager.instance.SetImapctFX(impulseSource, new Vector3(0.05f, 0.05f, 0), lookDirection, hitPause);
            else
                AttackPauseManager.instance.SetImapctFX(impulseSource, new Vector3(0.01f, 0, 0), lookDirection, 0);
        }

    }


    //private void OnGUI()
    //{
    //    Rect rect = new Rect(200, 200, 200, 200);
    //    string message = "Ԥ����: " + hasAttackInputBuffer;
    //    GUIStyle style = new GUIStyle();
    //    style.fontSize = 30;
    //    style.fontStyle = FontStyle.Bold;
    //    GUI.Label(rect, message, style);
    //}


    //public void SetAttackInputBufferTime()                  // ��������Ԥ����Э��
    //{
    //    StopCoroutine("AttackInputBufferCoroutine");
    //    StartCoroutine("AttackInputBufferCoroutine");
    //}

    //IEnumerator AttackInputBufferCoroutine()                // ����һ��ʱ��Ҫ�Զ�ȡ��Ԥ���룬��ֹ���簴��Ҳ�ܽ��빥��״̬
    //{
    //    hasAttackInputBuffer = true;
    //    yield return waitAttackInputBufferTime;
    //    hasAttackInputBuffer = false;
    //}




    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed *= 1 - _slowPercentage;
        jumpSpeed *= 1 - _slowPercentage;
        anim.speed *= 1 - _slowPercentage;

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpSpeed = defaultJumpSpeed;

    }

    public void AnimtionFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();// �ڵ�ǰ״̬��ȥ�������trigger�� triggerCalled = true; �����˳���ǰ״̬

    private void CheckForDash() // ����������������ʲô״̬���ܽ���dash��ֻҪ��ȴ����
    {

        if (IsWallDetected())
            return;

        // ���°��� && �ѽ��� && ��ȴ��� 
        if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.unlocked == true && skills.dash.CanUseSkill() || Input.GetKeyDown(KeyCode.JoystickButton2) && skills.dash.unlocked == true && skills.dash.CanUseSkill())
        {
            stateMachine.ChangeState(dashState);
        }
    }

    public override void SetDefaultKnockbackForce()
    {
        knockBackForce = new Vector2(3, 2);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    #region ��ײ���
    public virtual bool IsEdgeDetected() => Physics2D.Raycast(edgeCheck.position, Vector2.right * lookDirection, edgeCheckDistance, whatIsGround);

    // ���ߣ�ֻ�����ڿ��������е��Ӿ���ʾ����û��ʲôʵ������
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(interactionCheck.position, new Vector3(interactionCheck.position.x + interactionCheckDistance * lookDirection, interactionCheck.position.y));
        Gizmos.DrawLine(edgeCheck.position, new Vector3(edgeCheck.position.x + edgeCheckDistance * lookDirection, edgeCheck.position.y));
    }
    #endregion
}