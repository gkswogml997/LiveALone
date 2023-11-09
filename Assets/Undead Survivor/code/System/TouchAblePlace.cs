using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchAblePlace : MonoBehaviour
{
    public AimPointer aimPointer;
    bool isMouseClick;

    void Update()
    {
        if (isMouseClick)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimPointer.SetMousePosition(mousePosition);
        }
        else { aimPointer.SetMousePosition(Vector2.zero); }
    }

    public void PointDown()
    {
        isMouseClick = true;
    }
    public void PointUp()
    {
        isMouseClick = false;
    }
}
