using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class UpgradeWeaponItem: MonoBehaviour
{
    public Weapon weapon;
    public int makeGemId = 0;
    public int upradeGemId = 1;

    Image icon;
    Image outline;
    Text textLevel;
    Text textName;
    Text textDesc;
    Shop shop;
    

    private void Awake()
    {
        shop = GetComponentInParent<Shop>();
        outline = GetComponentsInChildren<Image>()[1];
        icon = GetComponentsInChildren<Image>()[2];
        outline.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        icon.sprite = weapon.GetComponent<SpriteRenderer>().sprite;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
    }

    public void UpToDateInfo()
    {
        textLevel.text = "Lv. " + weapon.level;
        textName.text = weapon.weaponName;
        textDesc.text = weapon.LevelUpDesc();
    }


    public void OnClick()
    {
        if (weapon.level == weapon.maxLevel) {
            shop.SetDialogMessage(shop.blacksmithDialog.fail);
            return;
        }

        InventoryItem requireGem = null;
        if (weapon.level == 0)
        {
            requireGem = GameManager.instance.player.InventoryFindItem(makeGemId);
        }
        else
        {
            requireGem = GameManager.instance.player.InventoryFindItem(upradeGemId);
        }

        if (requireGem == null)
        {
            shop.SetDialogMessage(shop.blacksmithDialog.moreGold);
        }
        else
        {
            shop.SetDialogMessage(shop.blacksmithDialog.buy);
            weapon.LevelUp();
            GameManager.instance.player.InventoryRemove(requireGem.itemId, 1);
        }
       
        UpToDateInfo();
    }
}
