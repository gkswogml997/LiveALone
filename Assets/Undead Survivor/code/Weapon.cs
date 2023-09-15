using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        switch (id)
        {
            
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;

                if (timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }

        if(Input.GetButtonDown("Jump"))
        {
            LevelUp(20, 5);
        }
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count = count;

        if (id == 0) { bullet_place(); }

        player.BroadcastMessage("ApplyGear",SendMessageOptions.DontRequireReceiver);
    }
    public void Init(ItemData data)
    {
        //기본설정
        name = "weapon " + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //프로퍼티
        id = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.instance.pool.gameObjects.Length; i++)
        {
            if (data.projectile == GameManager.instance.pool.gameObjects[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 150;
                bullet_place();
                break;
            default:
                speed = 0.3f;
                break;
        }

        //손 스프라이트 적용
        Hand hand = player.hands[(int)data.itemType];
        hand.spriteRenderer.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void bullet_place()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            {
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                bullet.parent = transform;
            }
            
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 vec_rot = Vector3.forward * 360 * i / count;
            bullet.Rotate(vec_rot);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero); // -1 is INF
        }
    }

    void Fire()
    {
        //exception
        if (player.scanner.nearest_target == null) {return;}

        //act
        Vector3 target_pos = player.scanner.nearest_target.position;
        Vector3 target_dir = target_pos - transform.position;
        target_dir = target_dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, target_dir);
        bullet.GetComponent<Bullet>().Init(damage, count, target_dir); // -1 is INF

    }
}

