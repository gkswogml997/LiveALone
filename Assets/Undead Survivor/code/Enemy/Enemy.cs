using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //몬스터 스텟
    public float speed;
    public float hp;
    public float maxhp;
    public float attack_point;

    //보상
    public int dropGold;

    //외부 컴포넌트 초기화
    public RuntimeAnimatorController[] runtimeAnimatorController;
    public Rigidbody2D target;

    //내부 상태 판별용 변수
    bool isLive;
    public bool isChargeHit;
    public GameObject hudDamageText;

    //내부 컴포넌트 초기화
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
        if (!isLive) { return; }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) { return; }
        if (!GameManager.instance.isLive) { return; }
        if (isChargeHit) { return; }

        //act
        Vector2 vec_dir = target.position - rigidbody.position;
        Vector2 vec_next = vec_dir.normalized * speed * Time.fixedDeltaTime;
        rigidbody.MovePosition(rigidbody.position + vec_next);
        rigidbody.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }
        if (!isLive) { return; }

        //act
        spriteRenderer.flipX = target.position.x < rigidbody.position.x;
    }
    //활성화 될 때
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        collider.enabled = true;
        rigidbody.simulated = true;
        spriteRenderer.sortingOrder += 1;

        animator.SetBool("Dead", false);
        hp = maxhp;
    }
    //초기화
    public void Init(SpawnData data)
    {
        //animator.runtimeAnimatorController = runtimeAnimatorController[data.sprite_type];
        dropGold = Random.Range(1, 150);
        speed = data.speed;
        maxhp = data.hp;
        hp = maxhp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //exception
        if (!collision.CompareTag("Bullet")) {return;}
        if (!isLive) { return; }

        //act
        float hit_damage = collision.GetComponent<RifleBullet>().damage;
        hp -= hit_damage;
        StartCoroutine(KnockBack());
        CreateDamageText(hit_damage);

        if (hp < 0) 
        {
            isLive = false;
            collider.enabled = false;
            rigidbody.simulated = false;
            spriteRenderer.sortingOrder -= 1;
            GameManager.instance.kill++;

            if(GameManager.instance.isLive) { AudioManager.instance.PlaySfx(AudioManager.SFX.Dead); }
            DropGold();
            animator.SetBool("Dead", true);
        }
        else
        {
            animator.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.SFX.Hit);
        }
    }

    IEnumerator KnockBack()
    {
        yield return waitForFixedUpdate; //1 물리 프레임 딜레이
        Vector3 player_pos = GameManager.instance.player.transform.position;
        Vector3 vec_dir = transform.position - player_pos;

        rigidbody.AddForce(vec_dir.normalized * 3, ForceMode2D.Impulse); // 3 is 넉백 강도

    }

    public IEnumerator BigKnockBack()
    {
        isChargeHit = true;
        
        Vector3 player_pos = GameManager.instance.player.transform.position;
        Vector3 vec_dir = transform.position - player_pos;

        rigidbody.AddForce(vec_dir.normalized * 30, ForceMode2D.Impulse); // 3 is 넉백 강도
        yield return new WaitForSeconds(0.25f);
        isChargeHit = false;

    }

    void CreateDamageText(float damage)
    {
        
        GameObject hudText = Instantiate(hudDamageText); // 생성할 텍스트 오브젝트
        hudText.transform.position = transform.position; // 표시될 위치
        hudText.GetComponentInChildren<DamageText>().damage = (int)damage; // 데미지 전달
        
    }

    void Dead() 
    {
        gameObject.SetActive(false);
    }

    void DropGold()
    {
        GameObject gold = GameManager.instance.pool.Get(3);
        gold.transform.position = transform.position;
        gold.GetComponent<Gold>().Init(dropGold);
    }
}
