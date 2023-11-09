using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShotGun : Weapon
{
    [Header("밸런스 수치")]
    public float damage = 1;
    public float speed = 1f;
    public float laserWidth = 1f;
    public int bulletId = 2;
    //;
    public float range = 1;
    public float bulletNumber = 10;

    float timer = 10;

    [Header("외부 컴포넌트 연결용")]
    public List<ShotGunLevelUpData> levelUpDataList;
    public ShotGunLevelDecs levelUpText;
    

    private void Awake()
    {
        firePosTransform = transform.GetChild(0);
        levelUpDataList = GameManager.instance.jsonLoader.ShotGun_levelUpDataList;
        levelUpText = GameManager.instance.jsonLoader.ShotGun_levelUpDesc;
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
                if (Input.GetMouseButton(0) && isInRange)
                {
                    
                    if (bulletCount > 0 && !isShooting) { StartCoroutine(ShotgunFire()); bulletCount--; }
                    timer = 0;
                }
                if (autoFire)
                {
                    if (player.aimPointer.aimingTarget != null && !isShooting)
                    {
                        if (bulletCount > 0) { StartCoroutine(ShotgunFire()); bulletCount--; }
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
        float minAngle = -30f;
        float maxAngle = 30f;
        float randomAngle = Random.Range(minAngle, maxAngle);
        Vector2 target_pos = player.aimPointer.transform.position;
        Vector2 target_dir = Quaternion.Euler(0f, 0f, randomAngle) * new Vector2(target_pos.x - transform.position.x, target_pos.y - transform.position.y);
        target_dir = target_dir.normalized;


        Transform bullet = GameManager.instance.pool.Get(bulletId).transform;
        bullet.position = firePosTransform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, target_dir) * Quaternion.Euler(0, 0, 90f); ;
        bullet.GetComponent<RifleBullet>().Init(damage * player.attackPower, 1, target_dir); // -1 is INF
        firePosTransform.GetComponent<FireEffect>().StartFireEffect();
    }

    IEnumerator ShotgunFire()
    {
        isShooting = true;
        PublishFireEvent(bulletCount); 
        AudioManager.instance.PlaySfx(AudioManager.SFX.ShotGun);
        bulletCount = (int)bulletNumber;
        while (bulletCount > 0)
        {
            Fire();
            bulletCount--;
            yield return new WaitForFixedUpdate();
        }
        isShooting = false;
    }

    public override string LevelUpDesc()
    {
        if (maxLevel <= level) { return "Max"; }
        if (level == 0) { return levelUpText.weaponDesc; }
        string returnStr = "";
        foreach (ShotGunLevelUpData levelData in levelUpDataList)
        {
            if (levelData.level == level + 1)
            {
                if (levelData.damageRise != 0)
                {
                    returnStr += string.Format(levelUpText.damage, levelData.damageRise);
                    returnStr += "\n";
                }
                if (levelData.range != 0)
                {
                    returnStr += string.Format(levelUpText.range, levelData.range);
                    returnStr += "\n";
                }
                if (levelData.bulletNumber != 0)
                {
                    returnStr += string.Format(levelUpText.bulletNumber, levelData.bulletNumber);
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
        foreach (ShotGunLevelUpData levelData in levelUpDataList) 
        { 
            if (levelData.level == level)
            {
                if (level == 1)
                {
                    damage = levelData.damageRise;
                    range = levelData.range;
                    bulletNumber = levelData.bulletNumber;
                }
                else
                {
                    damage += levelData.damageRise;
                    range -= levelData.range;
                    bulletNumber += levelData.bulletNumber;
                }
                
            }
        }
    }
}
