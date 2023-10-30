using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Shop;

public class SettingUI : MonoBehaviour
{
    Slider bgmVolumeBar;
    Slider seVolumeBar;
    Toggle showHintToggle;

    public bool isOpen = false;

    private void Awake()
    {
        bgmVolumeBar = GetComponentsInChildren<Slider>()[0];
        seVolumeBar = GetComponentsInChildren<Slider>()[1];
        showHintToggle = GetComponentInChildren<Toggle>();

        bgmVolumeBar.onValueChanged.AddListener(bgmVolumeChange);
        seVolumeBar.onValueChanged.AddListener(seVolumeChange);
        showHintToggle.onValueChanged.AddListener(ToggleSwitch);
    }

    public void Show()
    {
        bgmVolumeBar.value = AudioManager.instance.bgmVolume;
        seVolumeBar.value = AudioManager.instance.bgmVolume;
        showHintToggle.isOn = GameManager.instance.showHint;
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

    void bgmVolumeChange(float value)
    {
        AudioManager.instance.bgmVolume = value;
        AudioManager.instance.ChangeVolume();
    }
    void seVolumeChange(float value)
    {
        AudioManager.instance.sfxVolume = value;
        AudioManager.instance.ChangeVolume();
    }

    void ToggleSwitch(bool value)
    {
        GameManager.instance.showHint = value;
    }
}
