using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixed : MonoBehaviour
{
    private void Start()
    {
        ResolutionFix();
    }

    private void ResolutionFix()
    {
        // ���� ���� ����
        float targetWidthAspect = 9.0f;
        float targetHeightAspect = 16.0f;

        Camera.main.aspect = targetWidthAspect / targetHeightAspect;

        float widthRatio = (float)Screen.width / targetWidthAspect;
        float heightRatio = (float)Screen.height / targetHeightAspect;

        float heightadd = ((widthRatio / (heightRatio / 100)) - 100) / 200;
        float widthadd = ((heightRatio / (widthRatio / 100)) - 100) / 200;

        // ���������� 0���� ������ش�.
        if (heightRatio > widthRatio)
            widthRatio = 0.0f;
        else
            heightRatio = 0.0f;

        Camera.main.rect = new Rect(
            Camera.main.rect.x + Mathf.Abs(widthadd),
            Camera.main.rect.x + Mathf.Abs(heightadd),
            Camera.main.rect.width + (widthadd * 2),
            Camera.main.rect.height + (heightadd * 2));
    }

}
