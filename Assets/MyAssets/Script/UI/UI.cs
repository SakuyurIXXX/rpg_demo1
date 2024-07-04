using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    public static UI instance;

    [Header("EndScreen")]
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [Header("UI���")]
    public GameObject characterUI;
    public GameObject skillTreeUI;
    public GameObject backpackUI;
    public GameObject craftUI;
    public GameObject optionsUI;
    public GameObject inGameUI;

    [Header("��ʾ��")]
    public UI_ItemToolTip itemToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_BackpackToolTip backpackToolTip;
    public UI_CraftWindow craftWindow;


    [SerializeField] private UI_VolumeSlider[] volumeSettings;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        fadeScreen.gameObject.SetActive(true);

        itemToolTip.gameObject.SetActive(true); // ����Ҫ����ȻҪ��hierarchy���ֶ����������

        itemToolTip.HideToolTip();
        backpackToolTip.HideToolTip();

        SwitchTo(inGameUI);

    }

    void Update()
    {

        // ��������尴��esc����ֱ�ӹر����
        if (skillTreeUI.activeSelf || backpackUI.activeSelf || optionsUI.activeSelf)
        {

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SwitchTo(inGameUI);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);

        //if (Input.GetKeyDown(KeyCode.K))
        //    SwitchWithKeyTo(skillTreeUI);

        //if (Input.GetKeyDown(KeyCode.M))
        //    SwitchWithKeyTo(backpackUI);
    }

    // �ڲ˵�������������е�UI����л�
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; // Ҫ����fadeScreen����ɶ������

            if (fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(6, null);
            _menu.SetActive(true);

        }

        // �����˵�����ͣ��Ϸʱ��
        if (GameManager.instance != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    // ����������UI���
    public void SwitchWithKeyTo(GameObject _menu)
    {
        // ����Ѿ��ڵ�ǰUI��壬��ر�
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    // ��������
    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }

    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1f);
        restartButton.SetActive(true);
    }

    public void RestartGameButton() => GameManager.instance.RestartScene();

    public void LoadData(GameData _data)
    {
        foreach (KeyValuePair<string, float> pair in _data.volumeSettings)
        {
            foreach (UI_VolumeSlider item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.volumeSettings.Clear();

        foreach (UI_VolumeSlider item in volumeSettings)
        {
            _data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
