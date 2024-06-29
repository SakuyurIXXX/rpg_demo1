using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] UI_FadeScreen fadeScreen;

    private void OnTriggerEnter2D(Collider2D enemyCollision)
    {
        StartCoroutine(LoadScreenWithFadeEffect(1f));
    }



    IEnumerator LoadScreenWithFadeEffect(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneName);
    }
}
