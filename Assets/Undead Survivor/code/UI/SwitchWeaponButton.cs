using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchWeaponButton : MonoBehaviour
{
    public int weaponId;

    WeaponChangeUI weaponChangeUI;
    Weapon weapon;
    Player player;
    Image lockImage;
    Image image;


    private void Awake()
    {
        weaponChangeUI = GetComponentInParent<WeaponChangeUI>();
        player = GameManager.instance.player;
        weapon = player.equipmentWeaponList[weaponId];
        image = GetComponentsInChildren<Image>()[1];
        lockImage = GetComponentsInChildren<Image>()[2];
        image.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (weapon.level == 0) { lockImage.enabled = true; }
        else { lockImage.enabled = false; }
    }


    public void OnClick()
    {
        player.SwitchWeapon(weaponId);
        weaponChangeUI.SetText();
    }
}
