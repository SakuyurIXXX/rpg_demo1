using System.Collections;
using UnityEngine;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    damage,
    critChance,
    critPower,
    hp,
    armor,
    evasion,
    magicResistance,
    fireDamage,
    iceDamage,
    lightingDamage,
}
public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("属性数值")]
    public Stat strength; // +1伤害，+1%爆伤
    public Stat agility; // +1%闪避，+1暴击
    public Stat intelligence; // +1法伤，+1%魔法抵抗
    public Stat vitality; // +3生命

    [Header("攻击数值")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;  // 默认值:150%

    [Header("防御数值")]
    public Stat hp;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("魔法数值")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited; // 点燃a一下就死了
    public bool isChilled; // -20%护甲
    public bool isShocked; // -20%命中

    private float ailmentTimer; //所有元素异常的计时器
    [SerializeField] private float ailmentDuration = 3f;

    [Header("点燃相关")]
    private float igniteDamageCoolDown = 0.3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    [Header("杂项")]
    public float invincibleTimer;
    public bool isInvincible;

    public int currentHp;

    private bool isCrit;

    public System.Action onHpChanged;




    protected virtual void Start()
    {
        currentHp = GetMaxHpValue();
        critPower.SetDefaultValue(150);
        fx = GetComponent<EntityFX>();

    }

    protected virtual void Update()
    {
        ailmentTimer -= Time.deltaTime;
        igniteDamageTimer -= Time.deltaTime;
        invincibleTimer -= Time.deltaTime;

        CheckInvincible();

        if (ailmentTimer < 0)
        {
            isIgnited = false;
            isChilled = false;
            isShocked = false;
        }

        if (igniteDamageTimer < 0 && isIgnited)
            DoIgniteDamage();
    }




    #region 这段和ItemEffect有关，暂时没做后面的处理
    // 这段和ItemEffect有关，暂时没做后面的处理
    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }
    private IEnumerator StatModCoroutine(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _statToModify.RemoveModifier(_modifier);
    }
    #endregion

    public virtual void DoDamageTo(CharacterStats _targetStats, int _hitDirection)
    {


        if (CanAvoid(_targetStats)) // 闪避判断
        {
            return;
        }

        isCrit = false;

        // 伤害计算公式，初版：（基础伤害-护甲）* 爆伤
        int totalDamage = damage.GetValue() + strength.GetValue(); // 伤害 = 基础伤害 + 力量

        totalDamage = CheckTargetArmor(_targetStats, totalDamage); // 伤害 = 伤害 - 护甲

        if (CanCrit()) // 伤害 = 伤害 * 爆伤 
        {
            totalDamage = CalculateCritDamage(totalDamage);
            isCrit = true;

        }


        // 检查魔法伤害，区分与物理伤害，单独一个区间
        DoMagicalDamage(_targetStats, _hitDirection);

        // 用来区分伤害类型，改变伤害颜色
        if (isCrit)
            _targetStats.TakeDamage(totalDamage, _hitDirection, 0);
        else
            _targetStats.TakeDamage(totalDamage, _hitDirection, 4);

    }

    // hitDirection -1/1
    // damageType 0=crit,1=fire ,2=ice ,3=lighing ,4=normal
    public virtual void TakeDamage(int _damage, int _hitDirecition, int _damageType)
    {
        if (isInvincible)
            return;

        DecreaseHpBy(_damage);

        GetComponent<Entity>().DamageImpact(_hitDirecition); // 击退效果

        // 受到伤害弹出伤害文本，根据伤害类型改变文本的prefab
        if (_damage > 0)
            fx.CreatePopUpText(_damage.ToString(), _damageType);

        if (currentHp <= 0)
            Die();
    }

    // 直接造成HP减少的方法
    public virtual void DecreaseHpBy(int _damage)
    {
        if (isInvincible)
            return;
        currentHp -= _damage;

        if (onHpChanged != null)
            onHpChanged();
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats, int _hitDirection) // 目前等于各元素法伤总和+智力-(法抗+智力*3)，以后要改
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);



        // 元素伤害 <= 0 不造成元素异常
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _iceDamage && _lightingDamage > _fireDamage;

        // 元素伤害相等的情况，随机造成元素异常
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            switch (Random.Range(0, 2))
            {
                case 0:
                    canApplyIgnite = true;
                    _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                    break;

                case 1:
                    canApplyChill = true;
                    _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                    break;

                case 2:
                    canApplyShock = true;
                    _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                    break;

                default:
                    break;
            }
        }

        // 借用元素伤害大小的对比，区别造成的异常效果，以及受到的元素伤害类型
        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
            _targetStats.TakeDamage(totalMagicalDamage, _hitDirection, 1);

        }

        if (canApplyChill)
            _targetStats.TakeDamage(totalMagicalDamage, _hitDirection, 2);

        if (canApplyShock)
            _targetStats.TakeDamage(totalMagicalDamage, _hitDirection, 3);


        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    // 燃烧伤害
    private void DoIgniteDamage()
    {
        if (isInvincible)
            return;

        DecreaseHpBy(igniteDamage);

        if (igniteDamage > 0)
            fx.CreatePopUpText(igniteDamage.ToString(), 1);

        if (currentHp < 0)
            Die();
        igniteDamageTimer = igniteDamageCoolDown;
    }


    // 元素异常，造成控制
    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
            return;

        if (_ignite)
        {
            isIgnited = _ignite;
            ailmentTimer = ailmentDuration;


            fx.IgniteFxFor(ailmentDuration);
        }
        if (_chill)
        {
            isChilled = _chill;
            ailmentTimer = ailmentDuration;

            GetComponent<Entity>().SlowEntityBy(.2f, ailmentDuration);
            fx.ChillFxFor(ailmentDuration);

        }

        if (_shock)
        {
            isShocked = _shock;
            ailmentTimer = ailmentDuration;

            fx.ShockFxFor(ailmentDuration);
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    public virtual void Die()
    {
        isInvincible = true;
    }
    private bool CanAvoid(CharacterStats _targetStats) // 闪避率目前等于闪避率+敏捷，以后要改
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        // 麻痹-20%命中
        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage) // 护甲值目前直接等量减伤害，以后要改
    {
        // 目标麻痹-20%护甲
        if (_targetStats.isShocked)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // 造成伤害只能为正值 ，防止因为护甲减伤导致加血
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage) // 造成的法伤 -= 法抗 + 智力*3 
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    private bool CanCrit()// 暴击率目前等于暴击+敏捷，以后要改
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
            return true;
        return false;
    }

    private int CalculateCritDamage(int _damage) //爆伤目前等于爆伤+力量，以后要改
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritPower;
        return (Mathf.RoundToInt(critDamage));
    }

    public int GetMaxHpValue() // 生命值 = 基础生命 + 体力*3
    {
        return hp.GetValue() + vitality.GetValue() * 3;
    }

    public void SetInvincibleTime(float _invincibleTime) => invincibleTimer = _invincibleTime;
    public void MakeInvincible(bool _invincible) => isInvincible = _invincible;

    private void CheckInvincible()
    {
        if (invincibleTimer >= 0)
            isInvincible = true;
        else
            isInvincible = false;
    }

    public Stat GetStats(StatType _statType)
    {
        if (_statType == StatType.strength) return strength;
        else if (_statType == StatType.agility) return agility;
        else if (_statType == StatType.intelligence) return intelligence;
        else if (_statType == StatType.vitality) return vitality;
        else if (_statType == StatType.damage) return damage;
        else if (_statType == StatType.critChance) return critChance;
        else if (_statType == StatType.critPower) return critPower;
        else if (_statType == StatType.hp) return hp;
        else if (_statType == StatType.armor) return armor;
        else if (_statType == StatType.magicResistance) return magicResistance;
        else if (_statType == StatType.evasion) return evasion;
        else if (_statType == StatType.fireDamage) return fireDamage;
        else if (_statType == StatType.iceDamage) return iceDamage;
        else if (_statType == StatType.lightingDamage) return lightingDamage;

        return null;
    }
}
