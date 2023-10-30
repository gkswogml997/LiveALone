using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    public float damage;
    public float bulletSpeed;

    Rigidbody2D bulletRigid;
    Animator animator;

    private void Awake()
    {
        bulletRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void Init(float damage, Vector2 dir)
    {
        this.damage = damage;
        bulletRigid.velocity = dir * bulletSpeed;
        Invoke("SetAnimatorTrigger", 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //exception!
        if (!collision.CompareTag("Enemy")) { return; }

        //act
        collision.GetComponent<Enemy>().Hit(damage);
    }

    void SetAnimatorTrigger()
    {
        animator.SetTrigger("Finish");
    }

    private void Dead( )
    {
        gameObject.SetActive(false);
    }
}
