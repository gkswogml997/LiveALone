using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



public class Ak47 : Weapon
{
    [Header("밸런스 수치")]
    public float damage = 5;
    public int count = 3;
    public float speed = 1f;
    public float fireRate = 0.2f;
    public int fireCount = 3;
    public int bulletId = 2;
    

    float timer = 10;

    [Header("외부 컴포넌트 연결용")]
    
    public List<Ak47LevelUpData> levelUpDataList;
    public Ak47LevelDecs levelUpText;

    

    private void Awake()
    {
        firePosTransform = transform.GetChild(0);
        levelUpDataList = GameManager.instance.jsonLoader.Ak47_levelUpDataList;
        levelUpText = GameManager.instance.jsonLoader.Ak47_levelUpDesc;
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
        maxBulletCount = 30;
        bulletCount = maxBulletCount;
        reloadingTime = 0;
        reloadingTimeMax = 1f;
    }
    IEnumerator Shoot()
    {
        isShooting = true;

        int count = 0;
        while (count < fireCount)
        {
            if (bulletCount > 0) { Fire();  bulletCount--; }
            count++;
            yield return new WaitForSeconds(fireRate);
        }
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
        bullet.GetComponent<RifleBullet>().Init(damage * player.attackPower, count, target_dir); // -1 is INF
        firePosTransform.GetComponent<FireEffect>().StartFireEffect();
        PublishFireEvent(bulletCount);

        AudioManager.instance.PlaySfx(AudioManager.SFX.Range);
    }

    public override string LevelUpDesc()
    {
        if (maxLevel <= level) { return "Max"; }
        if (level == 0) { return levelUpText.weaponDesc; }
        string returnStr = "";
        foreach (Ak47LevelUpData levelData in levelUpDataList)
        {
            if (levelData.level == level + 1)
            {
                if (levelData.damageRise != 0)
                {
                    returnStr += string.Format(levelUpText.damage, levelData.damageRise);
                    returnStr += "\n";
                }
                if (levelData.perRise != 0)
                {
                    returnStr += string.Format(levelUpText.per, levelData.perRise);
                    returnStr += "\n";
                }
                if (levelData.speedRise != 0)
                {
                    returnStr += string.Format(levelUpText.speed, levelData.speedRise);
                    returnStr += "\n";
                }
                if (levelData.fireRateRise != 0)
                {
                    returnStr += string.Format(levelUpText.fireRate, levelData.fireRateRise);
                    returnStr += "\n";
                }
                if (levelData.fireCountRise != 0)
                {
                    returnStr += string.Format(levelUpText.fireCount, levelData.fireCountRise);
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
        foreach (Ak47LevelUpData levelData in levelUpDataList) 
        { 
            if (levelData.level == level)
            {
                if (level == 1)
                {
                    damage = levelData.damageRise;
                    count = levelData.perRise;
                    speed = levelData.speedRise;
                    fireRate = levelData.fireRateRise;
                    fireCount = levelData.fireCountRise;
                }
                else
                {
                    damage += levelData.damageRise;
                    count += levelData.perRise;
                    speed -= levelData.speedRise;
                    fireRate -= levelData.fireRateRise;
                    fireCount += levelData.fireCountRise;
                }
                
            }
        }
    }
}
