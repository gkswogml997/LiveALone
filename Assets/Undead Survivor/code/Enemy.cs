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
    Collider2D collider;
    Animator animator;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate waitForFixedUpdate;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //exception
        if (!is_live) { return; }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) { return; }

        //act
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
        collider.enabled = true;
        rigidbody.simulated = true;
        spriteRenderer.sortingOrder += 1;

        animator.SetBool("Dead", false);
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
        //exception
        if (!collision.CompareTag("Bullet")) {return;}
        if (!is_live) { return; }

        //act
        hp -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (hp < 0) 
        {
            is_live = false;
            collider.enabled = false;
            rigidbody.simulated = false;
            spriteRenderer.sortingOrder -= 1;
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            animator.SetBool("Dead", true);
        }
        else
        {
            animator.SetTrigger("Hit");
        }
    }

    IEnumerator KnockBack()
    {
        yield return waitForFixedUpdate; //1 ���� ������ ������
        Vector3 player_pos = GameManager.instance.player.transform.position;
        Vector3 vec_dir = transform.position - player_pos;

        rigidbody.AddForce(vec_dir.normalized * 3, ForceMode2D.Impulse); // 3 is �˹� ����

    }

    void Dead() 
    { 
        gameObject.SetActive(false);
    }
}
