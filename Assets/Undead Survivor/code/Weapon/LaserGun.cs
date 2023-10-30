using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;



public class LaserGun : Weapon
{
    [Header("밸런스 수치")]
    public float damage = 1;
    public float speed = 1f;
    public float laserWidth = 1f;
    public int bulletId = 2;
    

    float timer = 10;

    [Header("외부 컴포넌트 연결용")]
    public List<LaserGunLevelUpData> levelUpDataList;
    public LaserGunLevelDecs levelUpText;

    LineRenderer lineRenderer;

    private void Awake()
    {
        firePosTransform = transform.GetChild(0);
        levelUpDataList = GameManager.instance.jsonLoader.LaserGun_levelUpDataList;
        levelUpText = GameManager.instance.jsonLoader.LaserGun_levelUpDesc;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
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
                    if (bulletCount > 0 && !isShooting) { StartCoroutine(LaserFire()); bulletCount--; }
                    timer = 0;
                }
                if (autoFire)
                {
                    if (player.aimPointer.aimingTarget != null && !isShooting)
                    {
                        if (bulletCount > 0) { StartCoroutine(LaserFire()); bulletCount--; }
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
        Vector2 player_pos = firePosTransform.transform.position;
        Vector2 target_pos = player.aimPointer.transform.position;
        
        lineRenderer.SetPosition(0, player_pos);
        lineRenderer.SetPosition(1, target_pos);


        int layerMask = LayerMask.GetMask("Enemy");
        RaycastHit2D[] hit = Physics2D.LinecastAll(player_pos, target_pos, layerMask);
        foreach (RaycastHit2D raycastHit2D in hit)
        {
            if (raycastHit2D.transform.CompareTag("Enemy"))
            {
                raycastHit2D.transform.GetComponent<Enemy>().Hit(damage * player.attackPower);
            }
        }
        
    }

    IEnumerator LaserFire()
    {
        isShooting = true;
        lineRenderer.enabled = true;
        PublishFireEvent(bulletCount);
        AudioManager.instance.PlaySfx(AudioManager.SFX.Range);
        float laserLife = 0;
        while(laserLife < laserWidth) {
            Fire();
            laserLife += Time.deltaTime;
            yield return new WaitForFixedUpdate(); 
        }
        lineRenderer.enabled = false;
        isShooting = false;
    }

    public override string LevelUpDesc()
    {
        if (maxLevel <= level) { return "Max"; }
        if (level == 0) { return levelUpText.weaponDesc; }
        string returnStr = "";
        foreach (LaserGunLevelUpData levelData in levelUpDataList)
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
                if (levelData.laserWidth != 0)
                {
                    returnStr += string.Format(levelUpText.laserWidth, levelData.laserWidth);
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
        foreach (LaserGunLevelUpData levelData in levelUpDataList) 
        { 
            if (levelData.level == level)
            {
                if (level == 1)
                {
                    damage = levelData.damageRise;
                    speed = levelData.speedRise;
                    laserWidth = levelData.laserWidth;
                }
                else
                {
                    damage += levelData.damageRise;
                    speed -= levelData.speedRise;
                    laserWidth += levelData.laserWidth;
                }
                
            }
        }
    }
}
