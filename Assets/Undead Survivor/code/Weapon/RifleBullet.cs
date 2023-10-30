using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : MonoBehaviour
{
    public float damage;
    public int per;
    public float bulletSpeed;

    Rigidbody2D bulletRigid;

    private void Awake()
    {
        bulletRigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector2 dir)
    {
        this.damage = damage;
        this.per = per;
        bulletRigid.velocity = dir * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //exception!
        if (!collision.CompareTag("Enemy")) { return; }

        //act
        per--;
        collision.GetComponent<Enemy>().Hit(damage);
        if (per < 0)
        {
            bulletRigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //exception!
        if (!collision.CompareTag("Area") || per == -100) { return; }

        //act
        gameObject.SetActive(false);

    }
}
