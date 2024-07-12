using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string scene;
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

        SceneManager.LoadScene(scene);

        if (scene == "Cave")
        {
            collision.transform.position = new Vector2(-16f, -2f);
            AudioManager.instance.PlayBGM(0);
        }

        if (scene == "MainScene")
            collision.transform.position = new Vector2(56f, -4f);

    }
}
