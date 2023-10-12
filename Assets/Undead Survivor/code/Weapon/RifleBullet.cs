using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleBullet : MonoBehaviour
{
    public float damage;
    public int per;
    public float bulletSpeed = 15;

    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector2 dir)
    {
        this.damage = damage;
        this.per = per;
        rigidbody.velocity = dir * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //exception!
        if (!collision.CompareTag("Enemy")) { return; }

        //act
        per--;
        if (per < 0)
        {
            rigidbody.velocity = Vector2.zero;
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
