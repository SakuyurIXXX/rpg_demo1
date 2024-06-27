using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class UI_ToolTip : MonoBehaviour
{
    [SerializeField] private float xLimit = 960;
    [SerializeField] private float yLimit = 540;

    [SerializeField] private float xOffset = 50;
    [SerializeField] private float yOffset = 50;

    public virtual void AdjustPosition()
    {
        Vector2 mousePositon = Input.mousePosition;

        float newXOffset = 0;
        float newYOffset = 0;

        if (mousePositon.x > xLimit)
            newXOffset = -xOffset;
        else
            newXOffset = xOffset;

        if (mousePositon.y > yLimit)
            newYOffset = -yOffset;
        else
            newYOffset = yOffset;
        transform.position = new Vector2(mousePositon.x + newXOffset, mousePositon.y + newYOffset);
    }
}
