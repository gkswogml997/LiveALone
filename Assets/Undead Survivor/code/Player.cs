using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Searcher;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class InventoryItem
{
    public int itemId;
    public int quantity;
    public InventoryItem(int i, int q)
    {
        this.itemId = i;
        this.quantity = q;
    }
}

public class Player : MonoBehaviour
{
    [Header("# �÷��̾� ����")]
    public float hp;
    public float max_hp;
    public float speed;
    public float goldGainRange;
    public float chargeAttackPower;
    public List<InventoryItem> inventory;
    public int equipmentWeaponNumber;
    public Weapon[] equipmentWeaponList;

    public bool autoReload = true;
    public bool quickDraw = true;
    public bool bigMagazine = true;

    [Header("# ���� ����")]
    public bool isCharge = false;
    float chargeAttackTime = 0;

    [Header("# ����� ��ǲ")]
    public Vector2 vec_input;
    public Scanner scanner;
    public Hand[] hands;

    [Header("# ������Ʈ Get")]
    Rigidbody2D Rigidbody2D;
    SpriteRenderer spriteRenderer;
    Transform arms;
    Transform legs;
    Animator animator;
    //��
    SpriteRenderer armSpriteRenderer;
    //�ٸ�
    SpriteRenderer legSpriteRenderer;
    Animator legAnimator;

    void Awake()
    {
        hp = max_hp;
        equipmentWeaponNumber = 0;
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        hands = GetComponentsInChildren<Hand>(true);
        legs = transform.Find("PlayerLegs");
        arms = transform.Find("PlayerArms");
        armSpriteRenderer = arms.GetComponent<SpriteRenderer>();
        legSpriteRenderer = legs.GetComponent<SpriteRenderer>();
        legAnimator = legs.GetComponent<Animator>();
        equipmentWeaponList[equipmentWeaponNumber].SwitchWeapon(true);
        equipmentWeaponList[equipmentWeaponNumber].LevelUp();
    }

    void Update()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        vec_input.x = Input.GetAxisRaw("Horizontal");
        vec_input.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        //���� �ٲٱ�
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchWeapon(equipmentWeaponNumber - 1); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchWeapon(equipmentWeaponNumber + 1); }

        //��������
        if (!isCharge)
        {
            Vector2 vec_next = vec_input.normalized * speed * Time.fixedDeltaTime;
            Rigidbody2D.MovePosition(Rigidbody2D.position + vec_next);
            if (Input.GetMouseButtonDown(1)) { ChargeAttack(); }
        }
        else
        {
            chargeAttackTime += Time.deltaTime;
            if (chargeAttackTime > 0.2) { ChargeAttackEnd(); }
        }
    }

    void LateUpdate()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        

        //���콺 ������ �ٶ󺸵���
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 myPosition = transform.position;
        Vector3 direction = (mousePosition - myPosition).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float normalizedAngle = (angle + 360) % 360;
        spriteRenderer.flipX = normalizedAngle >= 90 && normalizedAngle < 270;

        //�ٸ��� ����
        legAnimator.SetFloat("Speed", vec_input.magnitude);
        if (vec_input.x != 0) 
        { 
            legSpriteRenderer.flipX = vec_input.x < 0; 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //exception
        if (!GameManager.instance.isLive) { return; }
        if (collision.transform.tag != "Enemy") { return; }

        //act
        if (!isCharge)
        {
            hp -= (Time.deltaTime * 10);
            if (hp <= 0)
            {
                for (int i = 2; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }

                animator.SetTrigger("Dead");
                GameManager.instance.GameOver();
            }
        }
        else
        {
            
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                // �� ��ũ��Ʈ�� �ִ� �ڷ�ƾ ȣ��
                if (enemy != null && !enemy.isChargeHit)
                {
                    enemy.StartCoroutine(enemy.BigKnockBack());
                }
            }
        }
    }

    public void ChangeStat(ShopItem.InfoType statId, float value)
    {
        switch (statId)
        {
            case ShopItem.InfoType.HP:
                max_hp += value;
                break;
        }
    }

    void ChargeAttack()
    {
        //���콺 �������� ����
        Vector2 mouseScreenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 target_pos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        Vector2 target_dir = new Vector2(target_pos.x - transform.position.x, target_pos.y - transform.position.y);
        target_dir = target_dir.normalized;
        Rigidbody2D.velocity = chargeAttackPower * Time.deltaTime * target_dir;
        //���콺 ������ �ٶ󺸱�
        float angle = Mathf.Atan2(target_dir.y, target_dir.x) * Mathf.Rad2Deg;
        float normalizedAngle = (angle + 360) % 360;
        legSpriteRenderer.flipX = normalizedAngle >= 90 && normalizedAngle < 270;
        //����ȭ
        PartsAlphaControl(0f);
        //�ִϸ��̼�
        legAnimator.SetBool("Sliding", true);
        isCharge = true;
    }
    void ChargeAttackEnd()
    {
        isCharge = false;
        PartsAlphaControl(1f);
        legAnimator.SetBool("Sliding", false); 
        chargeAttackTime = 0;
    }

    public void SwitchWeapon(int num)
    {
        if (num < 0) { return; }
        if (num >= equipmentWeaponList.Length) { return; }
        if (equipmentWeaponList[num].GetComponent<Weapon>().level == 0) { return;  }
        equipmentWeaponList[equipmentWeaponNumber].SwitchWeapon(false);
        equipmentWeaponList[num].SwitchWeapon(true);
        equipmentWeaponNumber = num;
    }

    void PartsAlphaControl(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
        color = armSpriteRenderer.color;
        color.a = alpha;
        armSpriteRenderer.color = color;
        color = equipmentWeaponList[equipmentWeaponNumber].GetComponent<SpriteRenderer>().color;
        color.a = alpha;
        equipmentWeaponList[equipmentWeaponNumber].GetComponent<SpriteRenderer>().color = color;
    }

    public InventoryItem InventoryFindItem(int id)
    {
        InventoryItem result = null;
        foreach (InventoryItem item in inventory)
        {
            if (item.itemId == id)
            {
                result = item;
                break;
            }
        }
        return result;
    }
    public void InventoryAdd(int id, int q)
    {
        bool search = false;
        foreach (InventoryItem item in inventory)
        {
            if (item.itemId == id)
            {
                item.quantity+=q;
                search = true;
                break;
            }
        }
        if (!search)
        {
            inventory.Add(new InventoryItem(id, q));
        }
    }

    public void InventoryRemove(int id, int q)
    {
        InventoryItem remove = null;
        foreach (InventoryItem item in inventory)
        {
            if (item.itemId == id)
            {
                item.quantity -= q;
                if (item.quantity <= 0) { remove = item; }
                break;
            }
        }
        if (remove != null)
        {
            inventory.Remove(remove);
        }
    }
}
