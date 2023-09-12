using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //���� ����
    public float speed;
    public float hp;
    public float maxhp;

    //�ܺ� ������Ʈ �ʱ�ȭ
    public RuntimeAnimatorController[] runtimeAnimatorController;
    public Rigidbody2D target;

    //���� ���� �Ǻ��� ����
    bool is_live;

    //���� ������Ʈ �ʱ�ȭ
    Rigidbody2D rigidbody;
    Animator animator;
    SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!is_live) { return; }
        Vector2 vec_dir = target.position - rigidbody.position;
        Vector2 vec_next = vec_dir.normalized * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + vec_next);
        rigidbody.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if(!is_live) { return; }
        spriteRenderer.flipX = target.position.x < rigidbody.position.x;
    }
    //Ȱ��ȭ �� ��
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        is_live = true;
        hp = maxhp;
    }
    //�ʱ�ȭ
    public void Init(SpawnData data)
    {
        animator.runtimeAnimatorController = runtimeAnimatorController[data.sprite_type];
        speed = data.speed;
        maxhp = data.hp;
        hp = maxhp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet")) {return;}
        hp -= collision.GetComponent<Bullet>().damage;

        if (hp < 0) {Dead();}
    }

    void Dead() 
    { 
        gameObject.SetActive(false);
    }
}
