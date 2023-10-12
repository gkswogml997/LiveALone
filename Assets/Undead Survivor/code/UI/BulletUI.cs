using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    public bool isCreate = true;
    private bool isFalling = false;
    private Vector3 startPos;
    public float floatTime = 0.5f;
    public float floatSpeed = 500f;

    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        if (isCreate) { CreateAnimation(); }
        else
        {
            Destroy(transform.gameObject);
        }
    }

    private void CreateAnimation()
    {
        rectTransform.Rotate(new Vector3(0, 0, 5));
        Vector3 myPos = rectTransform.position; // localPosition을 사용하여 위치 조정
        if (!isFalling)
        {
            myPos.y += floatSpeed * Time.deltaTime;
            rectTransform.position = myPos;
            floatTime -= Time.deltaTime;
            if (floatTime <= 0) { isFalling = true; }
        }
        else
        {
            myPos.y -= floatSpeed * Time.deltaTime;
            rectTransform.position = myPos;
            if (rectTransform.position.y < startPos.y) { isCreate = false; }
        }
    }


}
