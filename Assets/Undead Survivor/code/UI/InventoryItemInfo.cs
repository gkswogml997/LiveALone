using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfo : MonoBehaviour
{
    Image itemImage;
    Text itemName;
    Text itemDesc;

    private void Awake()
    {
        itemImage = GetComponentsInChildren<Image>()[1];
        itemName = GetComponentsInChildren<Text>()[0];
        itemDesc = GetComponentsInChildren<Text>()[1];
    }

    public void Init(Sprite spr, string name, string desc)
    {
        itemImage.sprite = spr;
        itemName.text = name;
        itemDesc.text = desc;
    }
}
