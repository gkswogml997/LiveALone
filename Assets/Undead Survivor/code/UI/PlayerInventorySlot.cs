using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    string itemName;
    string desc;

    Image itemImage;
    Text itemQuantity;
    public InventoryItemInfo infoUI;

    private void Awake()
    {
        itemImage = GetComponentsInChildren<Image>()[1];
        itemQuantity = GetComponentsInChildren<Text>()[0];
        infoUI = GameManager.instance.inventoryItemInfo;
    }

    public void Init(Sprite image, int q, string name, string desc)
    {
        this.itemName = name;
        this.desc = desc;
        if (image != null)
        {
            itemImage.enabled = true;
            itemImage.sprite = image;
            itemQuantity.text = "x" + q; 
        }
        else 
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
            itemQuantity.text = ""; 
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemImage.sprite == null) { return; }
        infoUI.gameObject.SetActive(true);
        infoUI.Init(itemImage.sprite, itemName, desc);
        infoUI.transform.position = transform.position;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemImage.sprite == null) { return; }
        infoUI.gameObject.SetActive(false);
    }
}
