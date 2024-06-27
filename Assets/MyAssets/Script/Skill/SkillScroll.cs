using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScroll : MonoBehaviour
{
    public string skillName; // 该道具解锁的技能名称

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<EntityFX>().CreatePopUpText(skillName + "已解锁", 4); // 视觉提示 
            AudioManager.instance.PlaySFX(8, null); // 音效提示
            UI.instance.SwitchWithKeyTo(UI.instance.skillTreeUI); // 打开SkillTreeUI

            Destroy(gameObject);

            // 对比卷轴中的技能名字，解锁对应技能
            if (skillName == "dash")
                SkillManager.instance.dash.UnlockDash();

            if (skillName == "counter")
                SkillManager.instance.counterAttack.UnlockCounter();
        }
    }
}
