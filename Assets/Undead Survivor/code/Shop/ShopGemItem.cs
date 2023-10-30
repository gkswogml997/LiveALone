using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopGemItem : MonoBehaviour
{
    public int price;

    Image icon;
    Text textName;
    Text textDesc;
    Text textPrice;
    Shop shop;
    GemData sellGem;
    List<GemData> uniqueItemList;
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
        uniqueItemList = new List<GemData>();

        foreach (GemData data in GameManager.instance.jsonLoader.gemDataList)
        {
            if (data.rating.Equals("unique"))
            {
                uniqueItemList.Add(data);
            }
        }

        ResetItem();
    }

    public void UpToDateInfo()
    {
        shopItemSprite = GameManager.instance.jsonLoader.gemSpriteList[sellGem.gemId];
        icon.sprite = shopItemSprite;
        textName.text = sellGem.textName;
        textDesc.text = sellGem.textDesc;
        textPrice.text = "" + price;
    }

    public void ResetItem()
    {
        int rand = Random.Range(0, uniqueItemList.Count);
        sellGem = uniqueItemList[rand];
        price = Random.Range(1000,2000) * GameManager.instance.gameLevel;
        UpToDateInfo();
    }

    public void onClick()
    {
        if (GameManager.instance.gold >= price)
        {
            GameManager.instance.gold -= price;
            shop.SetDialogMessage(shop.shopDialog.buy);
            GameManager.instance.player.InventoryAdd(sellGem.gemId, 1);
        }
        else
        {
            shop.SetDialogMessage(shop.shopDialog.fail);
        }
    }
}
