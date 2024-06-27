using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    private Player player;

    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage, int _hitDirecition, int _damageType)
    {
        base.TakeDamage(_damage, _hitDirecition, _damageType);

    }

    protected override void DecreaseHpBy(int _damage)
    {
        base.DecreaseHpBy(_damage);

        // 重伤（受到30%血以上的伤害）
        if (_damage > GetMaxHpValue() * .3f)
        {
            player.SetKnockbackForce(new Vector2(10, 5));
            AttackPauseManager.instance.SetImapctFX(player.impulseSource, new Vector3(0.3f, 0.3f, 0), player.lookDirection, 10);
        }
        else
            player.SetDefaultKnockbackForce();

    }

    public override void Die()
    {
        base.Die();
        player.Die();
    }
}
