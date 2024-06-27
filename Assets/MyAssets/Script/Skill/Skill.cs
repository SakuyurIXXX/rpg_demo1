using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{

    [SerializeField] public float coolDown;                         // ������ȴ
    protected float coolDownTimer;


    public bool unlocked;                                           // ���ܽ����ж�
    public UI_SkillTreeSlot skillTreeSlot;                          // ��Ӧ����������



    private void Start()
    {
        Invoke("CheckUnlocked", .1f);                              // ������ʱִ��,�ճ�����bug
    }
    protected virtual void Update()
    {
        coolDownTimer -= Time.deltaTime;
    }


    protected virtual void CheckUnlocked()                          // ��Ҫ��ÿ���������������д�������ڶ�����ʱ����ݼ��ܸ��ӵĽ���״��ʵ�ʽ�������ʹ��
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
