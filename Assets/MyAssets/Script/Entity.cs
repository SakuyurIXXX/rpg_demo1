using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    /// <summary>
    /// 所有实体的基类
    /// 所有实体都具有的基本属性和方法写在这里
    /// </summary>
    /// 
    public int lookDirection { get; private set; } = 1;
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx { get; private set; }
    public CharacterStats stats { get; private set; }

    public CapsuleCollider2D cd { get; private set; }
    #endregion

    [Header("被击退")]
    [SerializeField] protected Vector2 knockBackForce;
    protected int hitDirecition;
    [SerializeField] protected float knockBackDuration;
    protected bool isKnocked;
    public int hitPause;                                                    // 卡肉时间，目前是全局时间暂停，当然也可以不用这个变量自己定个值

    public System.Action onFlipped;

    #region 变量:碰撞检测相关
    [Header("碰撞检测")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    protected bool isGrounded;
    [Space]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    protected bool isWallDetected;
    [Space]
    public Transform attackCheck;
    public float attackCheckRadius;
    #endregion

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }
    protected virtual void Update()
    {
    }

    public void SetKnockbackForce(Vector2 _knockbackforce) => knockBackForce = _knockbackforce;

    public virtual void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact(int _hitDirecition)
    {
        hitDirecition = _hitDirecition;
        StartCoroutine("HitKnockback");
        fx.StartCoroutine("RedColorBlink");
    }

    protected virtual IEnumerator HitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockBackForce.x * hitDirecition, knockBackForce.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity); // 只有在调用SetVelocity()的时候实体才会翻转
    }

    public virtual void SetDefaultKnockbackForce()
    {

    }

    public virtual void Die()
    {
    }

    #region 碰撞检测
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * lookDirection, wallCheckDistance, whatIsGround);


    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * lookDirection, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }// 画线，只是用于开发调试中的视觉提示，并没有什么实际作用
    #endregion

    #region Flip

    public virtual void SetDefaultDirection(int _lookDirection)
    {
        lookDirection = _lookDirection;
    }
    public virtual void Flip()
    {
        lookDirection *= -1;
        transform.Rotate(0, 180, 0);
        if (onFlipped != null)
            onFlipped();
    }

    public virtual void FlipController(float _xVelocity)
    {
        if (lookDirection == 1 && _xVelocity < 0)
            Flip();
        if (lookDirection == -1 && _xVelocity > 0)
            Flip();
    }
    #endregion
}
