using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoUI : MonoBehaviour
{
    public Player player;
    Weapon weapon;
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

        for (int i = 0; i < 120; i++)
        {
            Vector3 bulletPos = new Vector3(64f + i * 8, 1f, 0f);
            GameObject bullet = Instantiate(prefab,transform.position+bulletPos,Quaternion.identity);
            bullet.transform.SetParent(transform,false);
            bulletList.Add(bullet);
            bullet.gameObject.SetActive(false);
        }
        FindObjectOfType<Weapon>().OnBulletFiredEvent += Fire;
        FindObjectOfType<Weapon>().ReloadingEvent += Init;
        Init();
    }

    public void Init()
    {
        SpriteRenderer spriteRenderer = weapon.GetComponent<SpriteRenderer>();
        weapon = player.equipmentWeaponList[player.equipmentWeaponNumber];
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
        Instantiate(usedBullet, bulletList[i-1].transform.position, Quaternion.identity).transform.SetParent(transform,false);
        bulletList[i - 1].SetActive(false);
    }


}
