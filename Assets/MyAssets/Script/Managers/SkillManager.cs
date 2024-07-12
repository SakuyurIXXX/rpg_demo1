using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public DashSkill dash;
    public CounterSkill counterAttack;
    public WallBounce wallBounce;

    [SerializeField] private string skillName;


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        counterAttack = GetComponent<CounterSkill>();
        wallBounce = GetComponent<WallBounce>();
    }


}
