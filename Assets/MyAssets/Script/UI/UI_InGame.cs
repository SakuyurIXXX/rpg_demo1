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

    [Header("���")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Image playerHp;

    [Header("��ʹ�õ���")]
    [SerializeField] private Image usableItem;
    [SerializeField] private Image coolDownImage;
    private ItemData_Equipment currentFlask;

    [Header("��")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate;

    private InventoryManager inventory;
    void Start()
    {
        if (playerStats != null)
            playerStats.onHpChanged += UpdateHealthUI;


        if (bossStats != null)
        {
            bossInfo.SetActive(false);
            bossStats.onHpChanged += UpdateBossHealthUI;

        }

        inventory = InventoryManager.instance;

    }

    void Update()
    {
        UpdateSoulsUI();
        UpdateUsableItemUI();

        if (boss != null)
            CheckBossUI();

    }

    private void UpdateUsableItemUI()
    {
        if (inventory.GetEquipment(EquipmentType.Flask) != null)
        {
            usableItem.gameObject.SetActive(true);

            currentFlask = inventory.GetEquipment(EquipmentType.Flask);

            usableItem.sprite = currentFlask.icon;
            coolDownImage.sprite = currentFlask.icon;

            CheckCoolDownOf(coolDownImage, currentFlask.itemCooldown);

            if (Input.GetKeyDown(KeyCode.F))
                SetCoolDownOf(coolDownImage);
        }
        else
        {
            usableItem.gameObject.SetActive(false);
        }
    }


    // ����BossѪ��UI�ĳ��ֺ���ʧ 
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

    // ����BOSSѪ��UI
    private void UpdateBossHealthUI()
    {
        deathBringerHpSlider.maxValue = bossStats.GetMaxHpValue();
        deathBringerHpSlider.value = bossStats.currentHp;
    }

    // ���»�UI
    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.instance.GetCurrentSouls())
            soulsAmount += Time.deltaTime * increaseRate;
        else
            soulsAmount = PlayerManager.instance.GetCurrentSouls();

        currentSouls.text = ((int)soulsAmount).ToString("#,#");
    }

    // ����Ѫ��UI
    private void UpdateHealthUI()
    {
        //float playerHpPercent = (float)playerStats.currentHp / playerStats.GetMaxHpValue();
        playerHp.fillAmount = (float)playerStats.currentHp / playerStats.GetMaxHpValue();
    }

    // ���� / ������ȴ����
    private void SetCoolDownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCoolDownOf(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
