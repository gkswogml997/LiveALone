using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [Header("�ܺ� ������Ʈ ����")]
    

    [Header("���� ������Ʈ ����")]
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
