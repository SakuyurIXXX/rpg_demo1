using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
    public bool bossFightBegun;

    public CinemachineImpulseSource impulseSource;

    [Header("法术")]
    [SerializeField] private GameObject spellPrefab;
    public int amountOfSpells;
    public float spellCoolDown;

    public float lastTimeCast;
    public float spellStateCoolDown;

    [Header("传送")]
    [SerializeField] private BoxCollider2D arena;
    [SerializeField] private Vector2 surroundingCheckSize;
    public float chanceToTeleport;
    public float defaultChanceToTeleport = 30;

    #region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        SetDefaultDirection(-1);
        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Dead", this);
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "Cast", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    protected override void Update()
    {
        base.Update();
        // 血量<=50%进入二阶段
        if (stats.currentHp <= stats.hp.GetValue() / 2)
        {
            chanceToTeleport = 80;
            amountOfSpells = 5;
            spellCoolDown = 0.6f;
            spellStateCoolDown = 5;
            idleTime = 0.3f;
        }

    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void CastSpell()
    {
        Player player = PlayerManager.instance.player;

        float xOffset = 0;

        // 如果玩家在移动（速度大于一定值），则预测玩家前进释放,否则在玩家当前位置放
        if (player.rb.velocity.x > 5)
            xOffset = player.lookDirection * 3;

        Vector3 spellPosition = new Vector3(player.transform.position.x + xOffset, player.transform.position.y + 1.7f);

        GameObject newSpell = Instantiate(spellPrefab, spellPosition, Quaternion.identity);
        newSpell.GetComponent<CastController>().SetupSpell(stats);
    }

    public bool CanDoSpellCast()
    {
        if (Time.time >= lastTimeCast + spellStateCoolDown)
        {
            lastTimeCast = Time.time;
            return true;
        }

        return false;
    }


    public bool CanTeleport()
    {
        if (Random.Range(0, 100) < chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }

        else
            return false;
    }

    public void FindPosition()
    {
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

        transform.position = new Vector3(x, y);
        transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

        if (!GroundBelow() || SomethingIsAround())
        {
            Debug.Log("周围有东西，重新传");
            FindPosition();
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
    private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }
}
