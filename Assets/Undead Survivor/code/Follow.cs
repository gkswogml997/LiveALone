using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [Header("외부 컴포넌트 연결")]
    

    [Header("내부 컴포넌트 연결")]
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rectTransform.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
