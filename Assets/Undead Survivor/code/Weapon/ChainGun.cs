using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ChainGun : Weapon
{
    [Header("밸런스 수치")]
    public float damage = 1;
    public float speed = 1f;
    public float laserWidth = 1f;
    public int shiftLife = 3;
    public float shiftRange = 50;
    public GameObject myBullet;

    public int targetNumber = 1;
    public int targetDir = 1;

    float timer = 10;

    [Header("외부 컴포넌트 연결용")]
    public List<ChainGunLevelUpData> levelUpDataList;
    public ChainGunLevelDecs levelUpText;
    

    private void Awake()
    {
        firePosTransform = transform.GetChild(0);
        levelUpDataList = GameManager.instance.jsonLoader.ChainGun_levelUpDataList;
        levelUpText = GameManager.instance.jsonLoader.ChainGun_levelUpDesc;
        Init();
    }
    void Update()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        timer += Time.deltaTime;

        if (timer > speed)
        {
            if (!isShooting && !isReloading)
            {
                if (Input.GetMouseButton(0))
                {
                    if (bulletCount > 0 && !isShooting) { Fire(); bulletCount--; }
                    timer = 0;
                }
                if (autoFire)
                {
                    if (player.aimPointer.aimingTarget != null && !isShooting)
                    {
                        if (bulletCount > 0) { Fire(); bulletCount--; }
                        timer = 0;
                    }
                }

            }
        }
        //자동 재장전
        if ((Input.GetMouseButton(0) || autoFire) && bulletCount <= 0 && player.autoReload)
        {
            if (!isReloading) { StartCoroutine(Reloading()); }
        }
        //수동 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading) { StartCoroutine(Reloading()); }
        }
    }

    void Init()
    {
        weaponName = levelUpText.weaponName;
        weaponDesc = levelUpText.weaponDesc;
        level = 0;
        maxLevel = levelUpDataList.Count;
        maxBulletCount = 5;
        bulletCount = maxBulletCount;
        reloadingTime = 0;
        reloadingTimeMax = 0.5f;
    }

    void Fire()
    {
        List<Transform> list = new List<Transform>();
        List<Transform> hitlist = new List<Transform>();
        list.Add(transform);
        GameObject bullet = Instantiate(myBullet, firePosTransform.transform.position, Quaternion.identity);
        bullet.GetComponent<ChainGunBullet>().Init(damage, shiftLife - 1, shiftRange, list, hitlist);

        PublishFireEvent(bulletCount);

        AudioManager.instance.PlaySfx(AudioManager.SFX.Range);
    }

    public override string LevelUpDesc()
    {
        if (maxLevel <= level) { return "Max"; }
        if (level == 0) { return levelUpText.weaponDesc; }
        string returnStr = "";
        foreach (ChainGunLevelUpData levelData in levelUpDataList)
        {
            if (levelData.level == level + 1)
            {
                if (levelData.damageRise != 0)
                {
                    returnStr += string.Format(levelUpText.damage, levelData.damageRise);
                    returnStr += "\n";
                }
                if (levelData.speedRise != 0)
                {
                    returnStr += string.Format(levelUpText.speed, levelData.speedRise);
                    returnStr += "\n";
                }
                if (levelData.targetNumber != 0)
                {
                    returnStr += string.Format(levelUpText.targetNumber, levelData.targetNumber);
                    returnStr += "\n";
                }
                if (levelData.targetDir != 0)
                {
                    returnStr += string.Format(levelUpText.targetDir, levelData.targetDir);
                    returnStr += "\n";
                }
            }
        }
        return returnStr;
    }
    public override void LevelUp()
    {
        if (maxLevel <= level) { return; }
        level++;
        foreach (ChainGunLevelUpData levelData in levelUpDataList) 
        { 
            if (levelData.level == level)
            {
                if (level == 1)
                {
                    damage = levelData.damageRise;
                    speed = levelData.speedRise;
                    shiftLife = levelData.targetNumber;
                    shiftRange = levelData.targetDir;
                }
                else
                {
                    damage += levelData.damageRise;
                    speed -= levelData.speedRise;
                    shiftLife += levelData.targetNumber;
                    shiftRange = levelData.targetDir;
                }
                
            }
        }
    }
}
