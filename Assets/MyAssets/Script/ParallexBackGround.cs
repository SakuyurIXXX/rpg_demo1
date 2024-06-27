using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallexBackGround : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] private float parallexEffect;
    private float xPosition;
    private float yPosition;
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
        yPosition = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distanceToMove = new Vector2(cam.transform.position.x * parallexEffect, cam.transform.position.y);

        transform.position = new Vector3(xPosition + distanceToMove.x, yPosition + distanceToMove.y);

    }
}
