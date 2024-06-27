using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [Header("Boss")]
    [SerializeField] private GameObject bossInfo;
    [SerializeField] private Slider deathBringerHpSlider;
    [SerializeField] private Enemy_DeathBringer boss;
    [SerializeField] private EnemyStats bossStats;

    [Header("玩家")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider playerHpSlider;

    [SerializeField] private Image dashImage;

    [Header("魂")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate;

    private SkillManager skills;
    void Start()
    {
        if (playerStats != null)
            playerStats.onHpChanged += UpdateHealthUI;


        if (bossStats != null)
        {
            bossInfo.SetActive(false);
            bossStats.onHpChanged += UpdateBossHealthUI;

        }

        skills = SkillManager.instance;

    }

    void Update()
    {
        UpdateSoulsUI();

        //if (Input.GetKeyDown(KeyCode.LeftShift) && skills.dash.unlocked)
        //    SetCoolDownOf(dashImage);

        //CheckCoolDownOf(dashImage, skills.dash.coolDown);
        if (boss != null)
            CheckBossUI();

    }


    // 控制Boss血条UI的出现和消失 
    private void CheckBossUI()
    {
        if (boss != null && boss.bossFightBegun == true)
            bossInfo.SetActive(true);
        else
            bossInfo.SetActive(false);

        if (bossStats.currentHp <= 0)
        {
            bossStats.onHpChanged -= UpdateBossHealthUI;
            bossInfo.SetActive(false);
        }
    }

    // 更新BOSS血条UI
    private void UpdateBossHealthUI()
    {
        deathBringerHpSlider.maxValue = bossStats.GetMaxHpValue();
        deathBringerHpSlider.value = bossStats.currentHp;
    }

    // 更新魂UI
    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrentSouls())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrentSouls();

        currentSouls.text = ((int)soulsAmount).ToString("#,#");
    }

    // 更新血条UI
    private void UpdateHealthUI()
    {
        playerHpSlider.maxValue = playerStats.GetMaxHpValue();
        playerHpSlider.value = playerStats.currentHp;
    }

    // 技能冷却动画
    //private void SetCoolDownOf(Image _image)
    //{
    //    if (_image.fillAmount <= 0)
    //        _image.fillAmount = 1;
    //}

    //private void CheckCoolDownOf(Image _image, float _cooldown)
    //{
    //    if (_image.fillAmount > 0)
    //        _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    //}
}
