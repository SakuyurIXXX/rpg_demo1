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
        myStats.onHpChanged += UpdateHealthUI; // ����Ѫ���¼�
        entity.onFlipped += FlipUI; //�����¼����������ý�ɫ��ת��ʱ��Ѫ��UI����ȥ
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
