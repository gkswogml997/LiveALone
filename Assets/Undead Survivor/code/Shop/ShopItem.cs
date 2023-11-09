using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Player player;
    public enum InfoType { HP, HPRecovery, Defence, Speed, AttackPower, GoldRange, GoldGrid, DropGrid, QuickDraw, BigMagazine, AutoReloading }

    public InfoType statId = 0;
    public int price = 100;
    public float value;
    public int myLevel;

    public bool isFixedPrice;
    public int fixedPrice;

    public int itemId = 0;
    public Image icon;

    
    bool isAct = true;

    Shop shop;
    Text textName;
    Text textDesc;
    Text textPrice;
    ShopItemData shopItemData;
    Sprite shopItemSprite;

    private void Awake()
    {
        shop = GetComponentInParent<Shop>();
        icon = GetComponentsInChildren<Image>()[1];
        Text[] texts = GetComponentsInChildren<Text>();
        textName = texts[0];
        textDesc = texts[1];
        textPrice = texts[2];
    }

    public void UpToDateInfo()
    {
        price = 50 + (10 * myLevel);

        if (isFixedPrice) { price = fixedPrice; }

        shopItemData = GameManager.instance.jsonLoader.shopItemDataList[itemId];
        shopItemSprite = GameManager.instance.jsonLoader.shopItemSpriteList[itemId];
        icon.sprite = shopItemSprite;
        textName.text = shopItemData.textName;
        textDesc.text = string.Format(shopItemData.textDesc, value);
        textPrice.text = "" + price;
        if (!isAct)
        {
            textPrice.text = "SoldOut";
        }
    }

    public void OnClick()
    {
        if (GameManager.instance.gold < price) { shop.SetDialogMessage(shop.shopDialog.moreGold); return; }
        if(!isAct) { shop.SetDialogMessage(shop.shopDialog.fail); return; }
        GameManager.instance.gold -= price;
        myLevel++;
        isAct = player.ChangeStat(statId, value);
        shop.SetDialogMessage(shop.shopDialog.buy);
        UpToDateInfo();
    }
}
