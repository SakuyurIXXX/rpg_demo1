using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTutorial : MonoBehaviour
{
    [SerializeField] private Transform tutorial;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            tutorial.gameObject.SetActive(true);
        }
        Invoke("ClosedTutorial", 5f);
    }

    public void ClosedTutorial()
    {
        tutorial.gameObject.SetActive(false);
    }
}
