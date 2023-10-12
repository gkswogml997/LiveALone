using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadingProgressBar : MonoBehaviour
{
    Slider mySilder;
    Weapon playerWeapon;

    private void Awake()
    {
        mySilder = GetComponent<Slider>();
    }

    public void Init(Weapon weapon)
    {
        playerWeapon = weapon;
    }

    void FixedUpdate()
    {
        float curHp = playerWeapon.reloadingTime;
        float maxHp = playerWeapon.reloadingTimeMax;
        mySilder.value = curHp / maxHp;
    }
}
