using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpText : MonoBehaviour
{
    private TextMeshPro myText;

    [SerializeField] private float speed;
    [SerializeField] private float disappearSpeed;                                      // ��ʧ�ٶ�
    [SerializeField] private float colorDisappearingSpeed;                              // ��͸�����ٶ�

    [SerializeField] private float lifeTime;

    private float textTimer;

    private void Start()
    {
        myText = GetComponent<TextMeshPro>();

        textTimer = lifeTime;
    }



    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y), speed * Time.deltaTime);
        textTimer -= Time.deltaTime;

        if (textTimer < 0)
        {
            float alpha = myText.color.a - colorDisappearingSpeed * Time.deltaTime;
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, alpha);

            if (myText.color.a < 50)
                speed = disappearSpeed;
            if (myText.color.a <= 0)
                Destroy(gameObject);
        }



    }


}