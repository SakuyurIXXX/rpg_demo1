using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneToChange;
    [SerializeField] UI_FadeScreen fadeScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            StartCoroutine(LoadScreenWithFadeEffect(1f, collision));
    }



    IEnumerator LoadScreenWithFadeEffect(float _delay, Collider2D collision)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);

        SceneManager.LoadScene(sceneToChange);

        if (sceneToChange == "Cave")
            collision.transform.position = new Vector2(-6.6f, -2f);

        if (sceneToChange == "MainScene")
            collision.transform.position = new Vector2(56f, -4f);

    }
}
