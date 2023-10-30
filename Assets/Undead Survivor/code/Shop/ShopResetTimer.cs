using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopResetTimer : MonoBehaviour
{
    Text timeText;
    private void Awake()
    {
        timeText = GetComponentsInChildren<Text>()[0];
    }

    private void LateUpdate()
    {
        timeText.text = "Reset\n" + (int)(GameManager.instance.shopResetTIme / 60) + " : " + (int)(GameManager.instance.shopResetTIme % 60);
        
    }
}
