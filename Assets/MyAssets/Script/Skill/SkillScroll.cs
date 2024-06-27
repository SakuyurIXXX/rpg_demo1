using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillScroll : MonoBehaviour
{
    public string skillName; // �õ��߽����ļ�������

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<EntityFX>().CreatePopUpText(skillName + "�ѽ���", 4); // �Ӿ���ʾ 
            AudioManager.instance.PlaySFX(8, null); // ��Ч��ʾ
            UI.instance.SwitchWithKeyTo(UI.instance.skillTreeUI); // ��SkillTreeUI

            Destroy(gameObject);

            // �ԱȾ����еļ������֣�������Ӧ����
            if (skillName == "dash")
                SkillManager.instance.dash.UnlockDash();

            if (skillName == "counter")
                SkillManager.instance.counterAttack.UnlockCounter();
        }
    }
}
