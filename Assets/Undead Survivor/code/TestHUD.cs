using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestHUD : MonoBehaviour
{
    public Text fpsText;
    private float deltaTime = 0.0f;

    private void Awake()
    {
        fpsText = GetComponent<Text>();
    }

    private void Update()
    {
        // FPS 계산
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        int roundedFPS = Mathf.RoundToInt(fps);

        // FPS 텍스트 업데이트
        fpsText.text = "FPS: " + roundedFPS;
    }
}
