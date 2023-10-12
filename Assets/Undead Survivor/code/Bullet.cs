using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector2 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
        {
            rigidbody.velocity = dir * 30;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //exception!
        if (!collision.CompareTag("Enemy") || per == -100) { return; }
        
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
