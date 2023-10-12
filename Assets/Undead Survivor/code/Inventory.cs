using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool isOpen = false;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Show()
    {
        rectTransform.localScale = Vector3.one;
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
}
