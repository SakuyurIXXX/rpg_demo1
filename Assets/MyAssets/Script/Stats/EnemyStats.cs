using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    private ItemDrop myDropSystem;
    public Stat soulsDropAmount;
    private bool isDropped;


    protected override void Start()
    {
        base.Start();
        enemy = GetComponent<Enemy>();
        myDropSystem = GetComponent<ItemDrop>();
        isDropped = false;
    }

    public override void TakeDamage(int _damage, int _hitDirecition, int _damageType)
    {
        base.TakeDamage(_damage, _hitDirecition, _damageType);

    }

    public override void Die()
    {
        // 消除实体用的是EnemyAnimationTrigger的DeadTrigger()

        base.Die();
        enemy.Die();

        PlayerManager.instance.souls += soulsDropAmount.GetValue();
        if (isDropped == false)
        {
            myDropSystem.GenerateDrop();
            isDropped = true;
        }

    }
}
