using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Player player;
    public string weaponName;
    public string weaponDesc;
    public int level;
    public int maxLevel; 
    public int bulletCount;
    public int maxBulletCount;
    public float reloadingTime;
    public float reloadingTimeMax;
    public bool isInRange = false;
    public bool isShooting = false;
    public bool isReloading = false;
    public bool autoFire = true;
    public Transform firePosTransform;
    public Transform autoFireTarget = null;


    public event Action<int> OnBulletFiredEvent;
    public event Action ReloadingEvent;

    public void PublishFireEvent(int count)
    {
        OnBulletFiredEvent?.Invoke(count);
    }
    public void PublishReloadingEvent()
    {
        ReloadingEvent?.Invoke();
    }

    public virtual string LevelUpDesc() { return null; }
    public virtual void LevelUp() { }

    public Transform ScanAutoTarget()
    {
        return player.scanner.nearest_target;
    }

    public void SwitchWeapon(bool isActivate)
    {
        gameObject.SetActive(isActivate);
        bulletCount = 0;
        reloadingTime = 0;
        isShooting = false;
        isReloading = false;
        if (isActivate) {StartCoroutine(Reloading());}
    }

    public IEnumerator Reloading()
    {
        isReloading = true;
        ReloadingProgressBar bar = GameManager.instance.player.transform.Find("Canvas").transform.Find("ReloadingBar").GetComponent<ReloadingProgressBar>();
        bar.gameObject.SetActive(true);
        bar.Init(this);
        while (reloadingTime < reloadingTimeMax)
        {
            yield return new WaitForFixedUpdate();
            reloadingTime += Time.deltaTime;
            if(player.quickDraw) { reloadingTime += Time.deltaTime; }
        }
        bar.gameObject.SetActive(false);
        bulletCount = maxBulletCount;
        if(player.bigMagazine) { bulletCount = maxBulletCount*2; }
        reloadingTime = 0;
        AudioManager.instance.PlaySfx(AudioManager.SFX.Land);
        PublishReloadingEvent();
        isReloading = false;
    }
}

