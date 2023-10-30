using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public enum ShopType { BlackSmith, Shop, Inventory};
    public ShopType type;
    public bool isOpen = false;


    RectTransform rectTransform;
    UpgradeWeaponItem[] weaponItems;
    ShopItem[] shopItems;
    ShopItemSell[] shopSellItems;
    ShopGemItem[] shopGemItems;
    PlayerInventory playerInventory;

    Text shopKeeperName;
    Text shopKeeperDialog;

    public ShopDialog shopDialog;
    public ShopDialog blacksmithDialog;
    public InventoryDialog inventoryDialog;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        weaponItems = GetComponentsInChildren<UpgradeWeaponItem>();
        shopItems = GetComponentsInChildren<ShopItem>();
        shopSellItems = GetComponentsInChildren<ShopItemSell>();
        shopGemItems = GetComponentsInChildren<ShopGemItem>();
        playerInventory = GetComponentInChildren<PlayerInventory>();
        shopKeeperName = transform.Find("Panel").Find("Portrait").Find("Name").GetComponent<Text>();
        shopKeeperDialog = transform.Find("Panel").Find("Portrait").Find("Dialog").GetComponent<Text>();

        switch (type)
        {
            case ShopType.Shop:
                shopDialog = GameManager.instance.jsonLoader.shopDialog;
                break;
            case ShopType.BlackSmith:
                blacksmithDialog = GameManager.instance.jsonLoader.blacksmithDialog;
                break;
            case ShopType.Inventory:
                inventoryDialog = GameManager.instance.jsonLoader.inventoryDialog;
                break;
        }
        
    }

    public void Show()
    {
        switch (type)
        {
            case ShopType.Shop:
                shopKeeperName.text = shopDialog.shopKeeperName;
                shopKeeperDialog.text = shopDialog.sayHi;
                break;
            case ShopType.BlackSmith:
                shopKeeperName.text = blacksmithDialog.shopKeeperName;
                shopKeeperDialog.text = blacksmithDialog.sayHi;
                break;
            case ShopType.Inventory:
                shopKeeperName.text = inventoryDialog.shopKeeperName;
                shopKeeperDialog.text = inventoryDialog.sayHi;
                break;
        }

        ListInfoUpdate();
        GameManager.instance.GameStop();
        isOpen = true;
        AudioManager.instance.PlaySfx(AudioManager.SFX.LevelUp);
        AudioManager.instance.EffectBGM(true);
    }

    public void Hide()
    {
        GameManager.instance.GameResume();
        isOpen = false;
        AudioManager.instance.PlaySfx(AudioManager.SFX.Select);
        AudioManager.instance.EffectBGM(false);
    }

    public void ListInfoUpdate()
    {
        foreach (UpgradeWeaponItem item in weaponItems)
        {
            item.UpToDateInfo();
        }
        foreach (ShopItem item in shopItems)
        {
            item.UpToDateInfo();
        }
        foreach (ShopItemSell item in shopSellItems)
        {
            item.UpToDateInfo();
        }
        foreach (ShopGemItem item in shopGemItems)
        {
            item.UpToDateInfo();
        }
        
        if (playerInventory != null) { playerInventory.UpToDateList(); }
    }

    public void ResetItems()
    {
        foreach(ShopItemSell item in shopSellItems)
        {
            item.ResetItem();
        }
        foreach (ShopGemItem item in shopGemItems)
        {
            item.ResetItem();
        }
    }

    public void SetDialogMessage(string text)
    {
        shopKeeperDialog.text = text;
    }
}
