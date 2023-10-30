using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopItemSell : MonoBehaviour
{
    public enum Rating  {common, rare, superRare, unique};
    public Rating rating;
    public Player player;

    public int price;

    Shop shop;
    Image icon;
    Text textName;
    Text textDesc;
    List<GemData> sellItemData;
    ShopItemData shopItemData;
    Sprite shopItemSprite;

    private void Awake()
    {
        shop = GetComponentInParent<Shop>();
        icon = GetComponentsInChildren<Image>()[1];
        Text[] texts = GetComponentsInChildren<Text>();
        textName = texts[0];
        textDesc = texts[1];
    }

    public void UpToDateInfo()
    {
        int itemId = (int)rating;
        shopItemData = GameManager.instance.jsonLoader.shopSellItemDataList[itemId];
        shopItemSprite = GameManager.instance.jsonLoader.shopSellItemSpriteList[itemId];
        sellItemData = GameManager.instance.jsonLoader.gemDataList;

        icon.sprite = shopItemSprite;
        textName.text = shopItemData.textName;
        textDesc.text = string.Format(shopItemData.textDesc, price);
    }

    public void OnClick()
    {
        List<InventoryItem> sellItem = new List<InventoryItem>();
        //등급에 맞는 아이템 찾기
        foreach(InventoryItem item in player.inventory)
        {
            Rating itemType = (Rating)Enum.Parse(typeof(Rating), sellItemData[item.itemId].rating);
            if (itemType == rating)
            {
                sellItem.Add(item);
            }
        }
        //찾은 아이템 전부 팔기
        if (sellItem.Count > 0)
        {
            int sellPrice = 0;
            foreach (InventoryItem item in sellItem)
            {
                sellPrice += price * item.quantity;
                player.InventoryRemove(item.itemId, item.quantity);
            }

            switch (rating)
            {
                case Rating.common:
                    shop.SetDialogMessage(shop.inventoryDialog.buy + "\nTotal: "+ sellPrice + " G");
                    break;
                case Rating.rare:
                    shop.SetDialogMessage(shop.inventoryDialog.buy2 + "\nTotal: " + sellPrice + " G");
                    break;
                case Rating.superRare:
                    shop.SetDialogMessage(shop.inventoryDialog.buy3 + "\nTotal: " + sellPrice + " G");
                    break;
            }

            GameManager.instance.gold += sellPrice;
        }
        else
        {
            shop.SetDialogMessage(shop.inventoryDialog.fail);
        }
       

        shop.ListInfoUpdate();
        UpToDateInfo();
    }

    public void ResetItem()
    {
        price = (int)(GameManager.instance.gameLevel * 10 * Random.Range(0.7f, 1.4f));
        if (rating == Rating.rare) { price *= 10; }
        if (rating == Rating.superRare) { price *= 100; }
        UpToDateInfo();
    }
}