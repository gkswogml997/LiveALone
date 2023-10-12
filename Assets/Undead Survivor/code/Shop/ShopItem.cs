using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Player player;
    public enum InfoType { HP, HPRecovery, Defence, Speed, AttackPower, GoldRange, GoldGrid, DropGrid, QuickDraw, BigMagazine, AutoReloading }

    public InfoType statId = 0;
    public int price = 10;
    public float value;
    public int myLevel;

    public int itemId = 0;
    public Image icon;
    Text textName;
    Text textDesc;
    Text textPrice;
    ShopItemData shopItemData;
    Sprite shopItemSprite;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        Text[] texts = GetComponentsInChildren<Text>();
        textName = texts[0];
        textDesc = texts[1];
        textPrice = texts[2];
    }

    public void UpToDateInfo()
    {
        price = 10 * myLevel;

        if (GameManager.instance.gold < price) { GetComponent<Button>().interactable = false; return; }
        else { GetComponent<Button>().interactable = true; }

        shopItemData = GameManager.instance.jsonLoader.shopItemDataList[itemId];
        shopItemSprite = GameManager.instance.jsonLoader.shopItemSpriteList[itemId];
        icon.sprite = shopItemSprite;
        textName.text = shopItemData.textName;
        textDesc.text = string.Format(shopItemData.textDesc, value);
        textPrice.text = "" + price;

    }

    void OnClick()
    {
        player.ChangeStat(statId, value);
    }
}
