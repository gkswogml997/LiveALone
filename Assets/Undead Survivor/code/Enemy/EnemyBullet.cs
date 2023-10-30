using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;

    Rigidbody2D bulletRigid;
    SpriteRenderer spriteRenderer;
    public Sprite[] sprite;

    private void Awake()
    {
        bulletRigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(float damage, int spr, Vector2 dir)
    {
        spriteRenderer.sprite = sprite[spr];
        this.damage = damage;
        bulletRigid.velocity = dir * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //exception!
        if (!collision.CompareTag("Player")) { return; }
        collision.transform.GetComponent<Player>().Hit(damage);
        gameObject.SetActive(false);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //exception!
        if (!collision.CompareTag("Area")) { return; }

        //act
        gameObject.SetActive(false);

    }
}
