using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class Pistol : Weapon
{
    [Header("밸런스 수치")]
    public float damage = 5;
    public float speed = 1f;
    public int bulletId = 2;
    
    float timer = 10;

    [Header("외부 컴포넌트 연결용")]
    public List<PistolLevelUpData> levelUpDataList;
    public PistolLevelDecs levelUpText;

    private void Awake()
    {
        firePosTransform = transform.GetChild(0);
        levelUpDataList = GameManager.instance.jsonLoader.Pistol_levelUpDataList;
        levelUpText = GameManager.instance.jsonLoader.Pistol_levelUpDesc;

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
                   
                    StartCoroutine(Shoot());
                    timer = 0;
                }
                if (autoFire)
                {
                    if (player.aimPointer.aimingTarget != null && player.aimPointer.aimingTarget.CompareTag("Enemy"))
                    {
                        StartCoroutine(Shoot());
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
        maxBulletCount = 6;
        bulletCount = maxBulletCount;
        reloadingTime = 0;
        reloadingTimeMax = 1f;
    }
    IEnumerator Shoot()
    {
        isShooting = true;
        if (bulletCount > 0) { Fire();  bulletCount--; }
        yield return new WaitForFixedUpdate();
        isShooting = false;
    }

    void Fire()
    {
        Vector2 target_pos = player.aimPointer.transform.position;
        Vector2 target_dir = new Vector2 (target_pos.x - transform.position.x, target_pos.y - transform.position.y);
        target_dir = target_dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(bulletId).transform;
        bullet.position = firePosTransform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, target_dir) * Quaternion.Euler(0, 0, 90f); ;
        bullet.GetComponent<RifleBullet>().Init(damage * player.attackPower, 0, target_dir); // -1 is INF
        firePosTransform.GetComponent<FireEffect>().StartFireEffect();
        PublishFireEvent(bulletCount);

        AudioManager.instance.PlaySfx(AudioManager.SFX.Pistol);
    }

    public override string LevelUpDesc()
    {
        if (maxLevel <= level) { return "Max"; }
        if (level == 0) { return levelUpText.weaponDesc; }
        string returnStr = "";
        foreach (PistolLevelUpData levelData in levelUpDataList)
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
            }
        }
        return returnStr;
    }
    public override void LevelUp()
    {
        if (maxLevel <= level) { return; }
        level++;
        foreach (PistolLevelUpData levelData in levelUpDataList) 
        { 
            if (levelData.level == level)
            {
                if (level == 1)
                {
                    damage = levelData.damageRise;
                    speed = levelData.speedRise;
                }
                else
                {
                    damage += levelData.damageRise;
                    speed -= levelData.speedRise;
                }
                
            }
        }
    }
}
