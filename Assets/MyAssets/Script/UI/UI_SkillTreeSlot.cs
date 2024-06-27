using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, ISaveManager, IPointerEnterHandler, IPointerExitHandler
{

    /// <summary>
    /// ����Hierarchy���Skill_UI���ţ���Ȼ���Ҳ������ISaveManager������û��s&l
    /// </summary>
    private UI ui;


    public bool unlocked;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;



    //[SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;                                   // ǰ�ü���
    //[SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;                                     // ͬ����֧����

    [SerializeField] private Color lockedSkillColor;
    [SerializeField] private Image skillImage;

    private void OnValidate()
    {
        gameObject.name = "Skill - " + skillName;
    }


    private void Start()
    {
        skillImage = GetComponent<Image>();
        ui = GetComponentInParent<UI>();
        ui.skillToolTip.HideToolTip();

        CheckUnlock();
        //Invoke("CheckUnlock", .1f);
    }

    private void CheckUnlock()                               // ������״̬���ı似�ܸ�����ɫ
    {
        // δ����
        if (unlocked == true)
            skillImage.color = Color.white;
        else
            skillImage.color = lockedSkillColor;
    }


    public void UnlockSkillSlot()                            // �������ܸ���
    {
        unlocked = true;
        skillImage.color = Color.white;

        // ���������𼶺ͷ�֧�������������������Ӧ��Ҫǰ�ü����ѽ�������ͬ����֧�����ѽ��������޷��ٽ����������
        //
        //for (int i = 0; i < shouldBeUnlocked.Length; i++)
        //{
        //    if (shouldBeUnlocked[i].unlocked == false)
        //    {
        //        Debug.Log("�޷�����");
        //        return;
        //    }
        //}

        //for (int i = 0; i < shouldBeLocked.Length; i++)
        //{
        //    if (shouldBeLocked[i].unlocked == true)
        //    {
        //        Debug.Log("�޷�����");
        //        return;
        //    }
        //}

    }

    public void OnPointerEnter(PointerEventData eventData)                               // ָ�������ʾ������ϸ��Ϣ��ʾ
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);
    }

    public void OnPointerExit(PointerEventData eventData)                                // ָ���˳����ؼ�����ϸ��ʾ
    {

        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
            unlocked = value;
    }

    public void SaveData(ref GameData _data)                                            // ����ֵ�����û��һ����ֵ���еĻ�Ҫɾ��
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
        {
            _data.skillTree.Remove(skillName);
            _data.skillTree.Add(skillName, unlocked);
        }
        else
            _data.skillTree.Add(skillName, unlocked);
    }
}
