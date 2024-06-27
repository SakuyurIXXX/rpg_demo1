using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : Entity
{
    /// <summary>
    /// 每有一个新的状态需要在这里声明和创建对象
    /// 玩家的基本属性和方法写在这里
    /// </summary>
    /// 

    //public bool hasAttackInputBuffer;
    //[SerializeField] private float attackInputBufferTime;
    //WaitForSeconds waitAttackInputBufferTime;

    private SkillManager skills;

    [Space]
    [HideInInspector] public int comboCounter;
    [HideInInspector] public bool isHit;                                       // 防止一次攻击造成多段伤害
    [HideInInspector] public bool isJumped;                                    // Coyote Time

    public float counterAttackDuration;                                        // 反击持续时间


    #region 碰撞检测相关
    [Header("碰撞检测")]

    // 用于检测可互动物品

    [SerializeField] protected Transform interactionCheck;
    [SerializeField] protected float interactionCheckDistance;

    // 用于角色边缘抓取
    [SerializeField] protected Transform edgeCheck;
    [SerializeField] protected float edgeCheckDistance;
    protected bool isEdge;
    #endregion  

    #region 移动相关
    [Header("移动")]
    public float moveSpeed;
    public float jumpSpeed;
    public float dashDuration;
    private float defaultMoveSpeed;
    private float defaultJumpSpeed;
    #endregion

    #region 打击感相关
    [Header("屏幕震动")]
    private Animator hitAnim;
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
        impulseSource = GetComponent<CinemachineImpulseSource>();

        skills = SkillManager.instance;
    }
    protected override void Update()
    {
        if (Time.timeScale == 0)
            return;

        base.Update();
        stateMachine.currentState.Update();
        CheckForDash();
        CheckForChest();

    }

    private void CheckForChest()
    {
        RaycastHit2D hit = Physics2D.Raycast(interactionCheck.position, Vector2.right * lookDirection, interactionCheckDistance, LayerMask.GetMask("Chest"));
        if (hit.collider != null)
        {

            Chest chest = hit.collider.GetComponent<Chest>();
            chest.interactableHint.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                chest.GenerateDrop();
            }
        }
    }

    //public void CheckAttackBuffer()
    //{
    //    if (isAttack && Input.GetKeyDown(KeyCode.Mouse0) || isAttack && Input.GetKeyDown(KeyCode.JoystickButton5))
    //        SetAttackInputBufferTime();
    //}

    public void CounterImpactFX()
    {
        hitAnim.SetTrigger("Hit"); // 打击特效

        AttackPauseManager.instance.SetImapctFX(impulseSource, new Vector3(0.05f, 0.05f, 0), lookDirection, 6);
    }



    //攻击碰撞检测 配合动画调整AttackArea使用
    private void OnTriggerEnter2D(Collider2D enemyCollision)
    {
        if (enemyCollision.CompareTag("Enemy") && isHit == false)
        {
            isHit = true;
            EnemyStats _target = enemyCollision.GetComponent<EnemyStats>();

            stats.DoDamage(_target, lookDirection);

            //Inventory.instance.GetEquipment(EquipmentType.Weapen).ExecuteItemEffect();


            hitAnim.SetTrigger("Hit"); // 打击特效
            if (comboCounter == 2)
                AttackPauseManager.instance.SetImapctFX(impulseSource, new Vector3(0.05f, 0.05f, 0), lookDirection, hitPause);
            else
                AttackPauseManager.instance.SetImapctFX(impulseSource, new Vector3(0.01f, 0, 0), lookDirection, 0);
        }

    }


    //private void OnGUI()
    //{
    //    Rect rect = new Rect(200, 200, 200, 200);
    //    string message = "预输入: " +  ;
    //    GUIStyle style = new GUIStyle();
    //    style.fontSize = 30;
    //    style.fontStyle = FontStyle.Bold;
    //    GUI.Label(rect, message, style);
    //}


    //public void SetAttackInputBufferTime()
    //{
    //    StopCoroutine("AttackInputBufferCoroutine");
    //    StartCoroutine("AttackInputBufferCoroutine");
    //}

    //IEnumerator AttackInputBufferCoroutine()
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

    public void AnimtionFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();// 在当前状态中去触发这个trigger（ triggerCalled = true; ）以退出当前状态

    private void CheckForDash() // 特例，基本上无论什么状态都能进入dash，只要冷却好了
    {

        if (IsWallDetected())
            return;

        // 按下按键 && 已解锁 && 冷却完毕 
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

    #region 碰撞检测
    public virtual bool IsEdgeDetected() => Physics2D.Raycast(edgeCheck.position, Vector2.right * lookDirection, edgeCheckDistance, whatIsGround);

    // 画线，只是用于开发调试中的视觉提示，并没有什么实际作用
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(interactionCheck.position, new Vector3(interactionCheck.position.x + interactionCheckDistance * lookDirection, interactionCheck.position.y));
        Gizmos.DrawLine(edgeCheck.position, new Vector3(edgeCheck.position.x + edgeCheckDistance * lookDirection, edgeCheck.position.y));
    }
    #endregion
}