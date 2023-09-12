using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //몬스터 스텟
    public float speed;
    public float hp;
    public float maxhp;

    //외부 컴포넌트 초기화
    public RuntimeAnimatorController[] runtimeAnimatorController;
    public Rigidbody2D target;

    //내부 상태 판별용 변수
    bool is_live;

    //내부 컴포넌트 초기화
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
    //활성화 될 때
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        is_live = true;
        hp = maxhp;
    }
    //초기화
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
