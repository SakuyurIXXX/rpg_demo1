using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity => GetComponentInParent<Entity>();
    private CharacterStats myStats => GetComponentInParent<CharacterStats>();
    private RectTransform myTransform;
    private Slider slider;

    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        UpdateHealthUI();
    }

    private void OnEnable()
    {
        myStats.onHpChanged += UpdateHealthUI; // 更新血条事件
        entity.onFlipped += FlipUI; //订阅事件，这里是让角色翻转的时候血条UI翻回去
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHpValue();
        slider.value = myStats.currentHp;
    }

    private void OnDisable()
    {
        if (entity != null)
            entity.onFlipped -= FlipUI;

        if (myStats != null)
            myStats.onHpChanged -= UpdateHealthUI;
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);

}
