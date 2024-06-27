using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    [SerializeField] public float coolDown;                         // 技能冷却
    protected float coolDownTimer;


    public bool unlocked;                                           // 技能解锁判断
    public UI_SkillTreeSlot skillTreeSlot;                          // 对应技能树格子



    private void Start()
    {
        Invoke("CheckUnlocked", .1f);                              // 必须延时执行,日常神秘bug
    }
    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }


    protected virtual void CheckUnlocked()                          // 需要在每个技能下面进行重写，用来在读档的时候根据技能格子的解锁状况实际解锁技能使用
    {
    }
    public virtual bool CanUseSkill()
    {
        if (coolDownTimer < 0)
        {
            UseSkill();
            coolDownTimer = coolDown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {
        // do something
    }
}
