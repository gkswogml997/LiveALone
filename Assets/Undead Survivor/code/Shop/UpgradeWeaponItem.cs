using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class UpgradeWeaponItem: MonoBehaviour
{
    public Weapon weapon;
    public int requireGemIds = 0;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
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

        InventoryItem requireGem = GameManager.instance.player.InventoryFindItem(requireGemIds);
        if (requireGem == null)
        {
            textDesc.text = "필요한 보석이 모자랍니다.";
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
        if (weapon.level == weapon.maxLevel)
        {
            GetComponent<Button>().interactable = false;
        }
    }


    public void OnClick()
    {
        weapon.LevelUp();
        GameManager.instance.player.InventoryRemove(requireGemIds, 1);
        UpToDateInfo();
    }
}
