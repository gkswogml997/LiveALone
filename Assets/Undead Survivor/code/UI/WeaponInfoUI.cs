using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoUI : MonoBehaviour
{
    public Player player;
    public Weapon weapon;
    public int weaponNum;
    Image weaponSprite;
    Text weaponName;

    [Header("ÃÑ¾Ë °ü·Ã")]
    public GameObject prefab;
    public GameObject usedBullet;
    public List<GameObject> bulletList;

    private void Awake()
    {
        weapon = player.equipmentWeaponList[player.equipmentWeaponNumber];
        weaponSprite = GetComponentsInChildren<Image>()[0];
        weaponName = GetComponentsInChildren<Text>()[0];


        for (int i = 0; i < 60; i++)
        {
            Vector3 bulletPos = new Vector3(30f + i * 2, -70f, 0f);
            GameObject bullet = Instantiate(prefab,transform.position+ bulletPos,Quaternion.Euler(0, 0, 45f));
            bullet.transform.SetParent(transform,false);
            bulletList.Add(bullet);
            bullet.gameObject.SetActive(false);
        }
        foreach(Weapon weapon in player.equipmentWeaponList)
        {
            weapon.OnBulletFiredEvent += Fire;
            weapon.ReloadingEvent += Init;
        }
        
        Init();
    }

    public void Init()
    {
        weaponNum = player.equipmentWeaponNumber;
        weapon = player.equipmentWeaponList[player.equipmentWeaponNumber];
        SpriteRenderer spriteRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponName.text = weapon.weaponName + " " + weapon.bulletCount;
        weaponSprite.sprite = spriteRenderer.sprite;
        for(int i = 0; i < bulletList.Count; i++)
        {
            if (i < weapon.bulletCount)
            {
                bulletList[i].SetActive(true);
            }else { bulletList[i].SetActive(false); }
        }
    }

    public void Fire(int i)
    {
        weapon = player.equipmentWeaponList[player.equipmentWeaponNumber];
        weaponName.text = weapon.weaponName + " " + (i - 1);
        Vector3 pos = new Vector3(30f + i * 2, -70f, 0f);
        Instantiate(usedBullet, transform.position + pos, Quaternion.Euler(0, 0, 45f)).transform.SetParent(transform,false);
        bulletList[i - 1].SetActive(false);
    }


}
