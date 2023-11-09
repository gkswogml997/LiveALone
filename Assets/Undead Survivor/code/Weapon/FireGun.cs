using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireGun : Weapon
{
    [Header("�뷱�� ��ġ")]
    public float damage = 1;
    public float speed = 1f;
    public int fireCount = 5;
    public int bulletId = 5;

    public float range = 1;
    public float dotDamage = 1;

    float timer = 10;

    [Header("�ܺ� ������Ʈ �����")]
    public List<FireGunLevelUpData> levelUpDataList;
    public FireGunLevelDecs levelUpText;
    

    private void Awake()
    {
        firePosTransform = transform.GetChild(0);
        levelUpDataList = GameManager.instance.jsonLoader.FireGun_levelUpDataList;
        levelUpText = GameManager.instance.jsonLoader.FireGun_levelUpDesc;
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
                    
                    if (bulletCount > 0 && !isShooting) { StartCoroutine(FireFire()); bulletCount--; }
                    timer = 0;
                }
                if (autoFire)
                {
                    if (player.aimPointer.aimingTarget != null && !isShooting)
                    {
                        if (bulletCount > 0) { StartCoroutine(FireFire()); bulletCount--; }
                        timer = 0;
                    }
                }

            }
        }
        //�ڵ� ������
        if ((Input.GetMouseButton(0) || autoFire) && bulletCount <= 0 && player.autoReload)
        {
            if (!isReloading) { StartCoroutine(Reloading()); }
        }
        //���� ������
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
        maxBulletCount = 1;
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
        bullet.GetComponent<FireBullet>().Init(damage * player.attackPower, target_dir); // -1 is INF
    }

    IEnumerator FireFire()
    {
        isShooting = true;
        AudioManager.instance.PlaySfx(AudioManager.SFX.FlameThrower);
        bulletCount = fireCount;
        while (bulletCount > 0)
        {
            Fire();
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
        foreach (FireGunLevelUpData levelData in levelUpDataList)
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
                if (levelData.dotDamage != 0)
                {
                    returnStr += string.Format(levelUpText.dotDamage, levelData.dotDamage);
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
        foreach (FireGunLevelUpData levelData in levelUpDataList) 
        { 
            if (levelData.level == level)
            {
                if (level == 1)
                {
                    damage = levelData.damageRise;
                    speed = levelData.range;
                    dotDamage = levelData.dotDamage;
                }
                else
                {
                    damage += levelData.damageRise;
                    speed -= levelData.range;
                    dotDamage += levelData.dotDamage;
                }
                
            }
        }
    }
}
