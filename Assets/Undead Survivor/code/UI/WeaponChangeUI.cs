using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using static Shop;

public class WeaponChangeUI : MonoBehaviour
{
    public bool isOpen = false;
    private SwitchWeaponButton[] buttons;
    Text text;

    private void Awake()
    {
        buttons = GetComponentsInChildren<SwitchWeaponButton>();
        text = GetComponentInChildren<Text>();
    }


    public void Show()
    {
        SetText();
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

    public void SetText()
    {
        Weapon weapon = GameManager.instance.player.GetPlayerWeapon();
        text.text = "Lv "+ weapon.level + "  " + weapon.weaponName;
    }

    public void ListInfoUpdate()
    {
        foreach(SwitchWeaponButton button in buttons)
        {
            button.UpdateInfo();
        }
    }
}
