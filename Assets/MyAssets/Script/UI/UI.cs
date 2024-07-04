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

    [Header("UI面板")]
    public GameObject characterUI;
    public GameObject skillTreeUI;
    public GameObject backpackUI;
    public GameObject craftUI;
    public GameObject optionsUI;
    public GameObject inGameUI;

    [Header("提示项")]
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

        itemToolTip.gameObject.SetActive(true); // 很重要，不然要在hierarchy里手动打开这个东西

        itemToolTip.HideToolTip();
        backpackToolTip.HideToolTip();

        SwitchTo(inGameUI);

    }

    void Update()
    {

        // 在任意面板按下esc都能直接关闭面板
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

    // 在菜单里点各个标题进行的UI面板切换
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null; // 要保持fadeScreen激活啥的来着

            if (fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            AudioManager.instance.PlaySFX(6, null);
            _menu.SetActive(true);

        }

        // 开启菜单后暂停游戏时间
        if (GameManager.instance != null)
        {
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
        }
    }

    // 按键开启的UI面板
    public void SwitchWithKeyTo(GameObject _menu)
    {
        // 如果已经在当前UI面板，则关闭
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

    // 死亡画面
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
