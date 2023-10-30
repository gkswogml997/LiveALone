using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClean : MonoBehaviour
{
    Rigidbody2D enemyCleaner;

    private void Awake()
    {
        enemyCleaner = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) { return; }
        Enemy enemy = collision.transform.GetComponent<Enemy>();
        enemy.Hit(enemy.maxhp);
    }
}
