using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;

    private void Start()
    {
        if (SaveManager.instance.HasSaveData() == false)
        {
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadScreenWithFadeEffect(1f));

    }

    public void NewGame()
    {
        StartCoroutine(LoadScreenWithFadeEffect(1f));
        SaveManager.instance.DeleteSaveData();
    }

    public void ChangeOptions()
    {

    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    IEnumerator LoadScreenWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }
}
