using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public bool isOpen = false;
    RectTransform rectTransform;
    UpgradeWeaponItem[] weaponItems;
    ShopItem[] shopItems;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        weaponItems = GetComponentsInChildren<UpgradeWeaponItem>();
        shopItems = GetComponentsInChildren<ShopItem>();
    }

    public void Show()
    {
        rectTransform.localScale = Vector3.one;
        ListInfoUpdate();
        GameManager.instance.GameStop();
        isOpen = true;
        AudioManager.instance.PlaySfx(AudioManager.SFX.LevelUp);
        AudioManager.instance.EffectBGM(true);
    }

    public void Hide()
    {
        rectTransform.localScale = Vector3.zero;
        GameManager.instance.GameResume();
        isOpen = false;
        AudioManager.instance.PlaySfx(AudioManager.SFX.Select);
        AudioManager.instance.EffectBGM(false);
    }

    public void ListInfoUpdate()
    {
        for (int i = 0; i < weaponItems.Length; i++)
        {
            weaponItems[i].UpToDateInfo();
        }
        for (int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].UpToDateInfo();
        }
    }
}
