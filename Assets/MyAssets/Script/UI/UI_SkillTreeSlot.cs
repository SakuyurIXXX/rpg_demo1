using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour, ISaveManager, IPointerEnterHandler, IPointerExitHandler
{

    /// <summary>
    /// 得在Hierarchy里把Skill_UI开着，不然它找不到这个ISaveManager，导致没法s&l
    /// </summary>
    private UI ui;


    public bool unlocked;
    [SerializeField] private string skillName;
    [TextArea]
    [SerializeField] private string skillDescription;



    //[SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;                                   // 前置技能
    //[SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;                                     // 同级分支技能

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

    private void CheckUnlock()                               // 检查解锁状态，改变技能格子颜色
    {
        // 未解锁
        if (unlocked == true)
            skillImage.color = Color.white;
        else
            skillImage.color = lockedSkillColor;
    }


    public void UnlockSkillSlot()                            // 解锁技能格子
    {
        unlocked = true;
        skillImage.color = Color.white;

        // 用来设置逐级和分支解锁，即解锁这个技能应该要前置技能已解锁；若同级分支技能已解锁，则无法再解锁这个技能
        //
        //for (int i = 0; i < shouldBeUnlocked.Length; i++)
        //{
        //    if (shouldBeUnlocked[i].unlocked == false)
        //    {
        //        Debug.Log("无法解锁");
        //        return;
        //    }
        //}

        //for (int i = 0; i < shouldBeLocked.Length; i++)
        //{
        //    if (shouldBeLocked[i].unlocked == true)
        //    {
        //        Debug.Log("无法解锁");
        //        return;
        //    }
        //}

    }

    public void OnPointerEnter(PointerEventData eventData)                               // 指针进入显示技能详细信息提示
    {
        ui.skillToolTip.ShowToolTip(skillDescription, skillName);
    }

    public void OnPointerExit(PointerEventData eventData)                                // 指针退出隐藏技能详细提示
    {

        ui.skillToolTip.HideToolTip();
    }

    public void LoadData(GameData _data)
    {
        if (_data.skillTree.TryGetValue(skillName, out bool value))
            unlocked = value;
    }

    public void SaveData(ref GameData _data)                                            // 检查字典里有没有一样的值，有的话要删除
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
