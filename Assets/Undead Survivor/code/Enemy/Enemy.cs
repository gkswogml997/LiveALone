using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    //몬스터 스텟
    public int monsterNumber;
    public float speed;
    public float hp;
    public float maxhp;
    public float attackPoint;
    public float attackRange;
    public float attackSpeed;
    float attackTimer;

    //보상
    public int dropGold;

    //외부 컴포넌트 초기화
    public RuntimeAnimatorController[] runtimeAnimatorController;
    public RuntimeAnimatorController[] bossRuntimeAnimatorController;
    public Rigidbody2D target;

    //내부 상태 판별용 변수
    public bool isBoss;
    public bool isRangeEnemy = true;
    bool isLive;
    public bool isChargeHit;

    //내부 컴포넌트 초기화
    Slider hpBar;
    Rigidbody2D enemyRigid;
    Collider2D enemyCollider;
    Animator animator;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate waitForFixedUpdate;
    


    void Awake()
    {
        enemyRigid = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hpBar = GetComponentInChildren<Slider>();
        
        waitForFixedUpdate = new WaitForFixedUpdate();
    }

    void FixedUpdate()
    {
        //exception
        if (!isLive) { return; }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) { return; }
        if (!GameManager.instance.isLive) { return; }
        if (isChargeHit) { return; }

        enemyRigid.velocity = Vector2.zero;

        //act
        if (Vector2.Distance(target.position, transform.position) >= attackRange)
        {
            Vector2 vec_dir = target.position - enemyRigid.position;
            Vector2 vec_next = vec_dir.normalized * speed * Time.fixedDeltaTime;
            
            enemyRigid.MovePosition(enemyRigid.position+vec_next);
            animator.SetBool("Walk", true);
        }
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackSpeed)
            {
                Attack();
                attackTimer = 0;
            }
            animator.SetBool("Walk", false);
        }

        spriteRenderer.flipX = target.position.x < enemyRigid.position.x;
    }

    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        hpBar.gameObject.SetActive(false);

        monsterNumber = 0;
        isBoss = false;
        transform.localScale = Vector3.one * 2;
        isLive = true;
        enemyCollider.enabled = true;
        enemyRigid.simulated = true;
        spriteRenderer.sortingOrder += 1;

        animator.SetBool("Dead", false);
        hp = maxhp;
    }
    //초기화
    public void Init(MonsterData data)
    {
        int gameLevel = GameManager.instance.gameLevel;
        monsterNumber = data.monsterNumber;
        animator.runtimeAnimatorController = runtimeAnimatorController[data.monsterNumber];
        dropGold = Random.Range(10, 15) * gameLevel;
        speed = 1 * data.speed;
        maxhp = 10 * gameLevel * data.hp;
        attackPoint = 2 * gameLevel * data.attackPoint;
        attackSpeed = 2 * data.attackSpeed;
        attackRange = 10 * data.range;

        if (isBoss)
        {
            animator.runtimeAnimatorController = bossRuntimeAnimatorController[data.monsterNumber];
            transform.localScale = Vector3.one * 4;
            maxhp *= gameLevel;
            attackPoint *= gameLevel;
        }

        hp = maxhp; 
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
    }

    public void RangeAttack(int bulletId)
    {
        Vector2 target_pos = GameManager.instance.player.transform.position;
        Vector2 target_dir = new Vector2(target_pos.x - transform.position.x, target_pos.y - transform.position.y);
        target_dir = target_dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(6).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, target_dir) * Quaternion.Euler(0, 0, 90f); ;
        bullet.GetComponent<EnemyBullet>().Init(attackPoint, bulletId, target_dir);
    }

    public void BossAttack(int spr)
    {
        for(int i = 0; i < 360; i+= 30) 
        {
            Vector2 target_dir = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad));
            Transform bullet = GameManager.instance.pool.Get(6).transform;
            bullet.position = transform.position;  
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, target_dir) * Quaternion.Euler(0, 0, 90f); ;
            bullet.GetComponent<EnemyBullet>().Init(attackPoint, spr, target_dir);
        }
    }

    public void Hit(float damage)
    {
        hp -= damage;
        StartCoroutine(KnockBack());
        CreateDamageText(damage);

        if (hp <= 0)
        {
            hpBar.gameObject.SetActive(false);
            isLive = false;
            enemyCollider.enabled = false;
            enemyRigid.simulated = false;
            spriteRenderer.sortingOrder -= 1;
            GameManager.instance.score += (100 * monsterNumber) + (10000 * (isBoss ? 1 : 0));

            if (GameManager.instance.isLive) { AudioManager.instance.PlaySfx(AudioManager.SFX.Dead); }
            DropGold();
            animator.SetBool("Dead", true);
        }
        else
        {
            if (hpBar.gameObject.activeSelf == false) { hpBar.gameObject.SetActive(true); }
            hpBar.value = hp / maxhp;
            animator.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.SFX.Hit);
        }
    }

    IEnumerator KnockBack()
    {
        yield return waitForFixedUpdate; //1 물리 프레임 딜레이
        enemyRigid.velocity = Vector2.zero;
        Vector3 player_pos = GameManager.instance.player.transform.position;
        Vector3 vec_dir = transform.position - player_pos;

        enemyRigid.AddForce(vec_dir.normalized * 3, ForceMode2D.Impulse); // 3 is 넉백 강도

    }

    public IEnumerator BigKnockBack()
    {
        isChargeHit = true;
        animator.SetTrigger("Hit");
        enemyRigid.velocity = Vector2.zero;
        Vector3 player_pos = GameManager.instance.player.transform.position;
        Vector3 vec_dir = transform.position - player_pos;

        enemyRigid.AddForce(vec_dir.normalized * 30, ForceMode2D.Impulse); // 3 is 넉백 강도
        yield return new WaitForSeconds(0.25f);
        isChargeHit = false;

    }

    void CreateDamageText(float damage)
    {
        
        GameObject hudText = GameManager.instance.pool.Get(5); ; // 생성할 텍스트 오브젝트
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
        gold.GetComponent<Gold>().Init(dropGold, false);
    }
}
