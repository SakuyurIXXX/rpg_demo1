using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexBackGround : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallexEffect;
    [SerializeField] private bool followY;

    private float xPosition;
    private float yPosition;

    private float backgroundLength;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        yPosition = transform.position.y;


        backgroundLength = GetComponent<SpriteRenderer>().bounds.size.x / 3;
    }

    void Update()
    {
        // ±³¾°ÎÞÏÞÑÓÉê
        float distanceMoved = cam.transform.position.x * (1 - parallexEffect);

        if (distanceMoved > xPosition + backgroundLength)
            xPosition += backgroundLength;
        else if (distanceMoved < xPosition - backgroundLength)
            xPosition -= backgroundLength;



        Vector2 distanceToMove = new Vector2(cam.transform.position.x * parallexEffect, cam.transform.position.y);

        // ÊÇ·ñ¹Ì¶¨yÖá
        if (followY)
            transform.position = new Vector3(xPosition + distanceToMove.x, yPosition + distanceToMove.y);
        else
            transform.position = new Vector3(xPosition + distanceToMove.x, yPosition);

    }
}
