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

    [Header("������ֵ")]
    public Stat strength; // +1�˺���+1%����
    public Stat agility; // +1%���ܣ�+1����
    public Stat intelligence; // +1���ˣ�+1%ħ���ֿ�
    public Stat vitality; // +3����

    [Header("������ֵ")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;  // Ĭ��ֵ:150%

    [Header("������ֵ")]
    public Stat hp;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("ħ����ֵ")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited; // ��ȼaһ�¾�����
    public bool isChilled; // -20%����
    public bool isShocked; // -20%����

    private float ailmentTimer; //����Ԫ���쳣�ļ�ʱ��
    [SerializeField] private float ailmentDuration = 3f;

    [Header("��ȼ���")]
    private float igniteDamageCoolDown = 0.3f;
    private float igniteDamageTimer;
    private int igniteDamage;

    [Header("����")]
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




    #region ��κ�ItemEffect�йأ���ʱû������Ĵ���
    // ��κ�ItemEffect�йأ���ʱû������Ĵ���
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


        if (CanAvoid(_targetStats)) // �����ж�
        {
            return;
        }

        isCrit = false;

        // �˺����㹫ʽ�����棺�������˺�-���ף�* ����
        int totalDamage = damage.GetValue() + strength.GetValue(); // �˺� = �����˺� + ����

        totalDamage = CheckTargetArmor(_targetStats, totalDamage); // �˺� = �˺� - ����

        if (CanCrit()) // �˺� = �˺� * ���� 
        {
            totalDamage = CalculateCritDamage(totalDamage);
            isCrit = true;

        }


        // ���ħ���˺��������������˺�������һ������
        DoMagicalDamage(_targetStats, _hitDirection);

        // ���������˺����ͣ��ı��˺���ɫ
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

        GetComponent<Entity>().DamageImpact(_hitDirecition); // ����Ч��

        // �ܵ��˺������˺��ı��������˺����͸ı��ı���prefab
        if (_damage > 0)
            fx.CreatePopUpText(_damage.ToString(), _damageType);

        if (currentHp <= 0)
            Die();
    }

    // ֱ�����HP���ٵķ���
    public virtual void DecreaseHpBy(int _damage)
    {
        if (isInvincible)
            return;
        currentHp -= _damage;

        if (onHpChanged != null)
            onHpChanged();
    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats, int _hitDirection) // Ŀǰ���ڸ�Ԫ�ط����ܺ�+����-(����+����*3)���Ժ�Ҫ��
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);



        // Ԫ���˺� <= 0 �����Ԫ���쳣
        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _iceDamage && _lightingDamage > _fireDamage;

        // Ԫ���˺���ȵ������������Ԫ���쳣
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

        // ����Ԫ���˺���С�ĶԱȣ�������ɵ��쳣Ч�����Լ��ܵ���Ԫ���˺�����
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

    // ȼ���˺�
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


    // Ԫ���쳣����ɿ���
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
    private bool CanAvoid(CharacterStats _targetStats) // ������Ŀǰ����������+���ݣ��Ժ�Ҫ��
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        // ���-20%����
        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage) // ����ֵĿǰֱ�ӵ������˺����Ժ�Ҫ��
    {
        // Ŀ�����-20%����
        if (_targetStats.isShocked)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue); // ����˺�ֻ��Ϊ��ֵ ����ֹ��Ϊ���׼��˵��¼�Ѫ
        return totalDamage;
    }
    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage) // ��ɵķ��� -= ���� + ����*3 
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    private bool CanCrit()// ������Ŀǰ���ڱ���+���ݣ��Ժ�Ҫ��
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();
        if (Random.Range(0, 100) <= totalCriticalChance)
            return true;
        return false;
    }

    private int CalculateCritDamage(int _damage) //����Ŀǰ���ڱ���+�������Ժ�Ҫ��
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritPower;
        return (Mathf.RoundToInt(critDamage));
    }

    public int GetMaxHpValue() // ����ֵ = �������� + ����*3
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
