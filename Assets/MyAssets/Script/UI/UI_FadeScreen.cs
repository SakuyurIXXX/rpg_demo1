using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeIn() => anim.SetTrigger("fadein");

    public void FadeOut() => anim.SetTrigger("fadeout");
}
