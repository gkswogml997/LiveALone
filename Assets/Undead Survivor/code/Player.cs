using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.Searcher;
using UnityEngine.InputSystem;
using UnityEngine;

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
    public float hpRecovery = 0.1f;
    public float attackPower = 3;
    public float defencePower = 1;
    public float speed;
    public float goldGainRange;
    public float chargeAttackPower;
    public List<InventoryItem> inventory;
    public int equipmentWeaponNumber;
    public Weapon[] equipmentWeaponList;

    public bool autoReload = true;
    public bool quickDraw = false;
    public bool bigMagazine = false;

    [Header("# ���� ����")]
    public bool isCharge = false;
    float chargeAttackTime = 0;

    [Header("# ����� ��ǲ")]
    public Vector2 vec_input;
    public Scanner scanner;
    public AimPointer aimPointer;

    [Header("# ������Ʈ Get")]
    Rigidbody2D Rigidbody2D;
    SpriteRenderer spriteRenderer;
    Transform arms;
    Transform legs;
    //��
    SpriteRenderer armSpriteRenderer;
    //�ٸ�
    SpriteRenderer legSpriteRenderer;
    Animator legAnimator;

    void Awake()
    {
        hp = max_hp;
        aimPointer = GetComponentInChildren<AimPointer>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        scanner = GetComponent<Scanner>();
        legs = transform.Find("PlayerLegs");
        arms = transform.Find("PlayerArms");
        armSpriteRenderer = arms.GetComponent<SpriteRenderer>();
        legSpriteRenderer = legs.GetComponent<SpriteRenderer>();
        legAnimator = legs.GetComponent<Animator>();
    }

    void OnMove(InputValue value)
    {
        vec_input = value.Get<Vector2>();
    }

    private void OnTouchPosition(InputAction.CallbackContext context)
    {
        Debug.Log("���콺 ��ġ �޴���");
        Vector2 touchPosition = context.ReadValue<Vector2>();
        aimPointer.SetMousePosition(touchPosition);
    }
    void FixedUpdate()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //���콺����Ʈ ����
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimPointer.SetMousePosition(mousePos);

        //act
        //���� �ٲٱ�
        if (Input.GetKeyDown(KeyCode.Alpha1)) { SwitchWeapon(equipmentWeaponNumber - 1); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { SwitchWeapon(equipmentWeaponNumber + 1); }

        //ü�����
        hp = Mathf.Clamp(hp, 0, max_hp);
        if (hp < max_hp)
        {
            hp += hpRecovery;
        }

        //��������
        if (!isCharge)
        {
            Vector2 vec_next = vec_input * speed * Time.fixedDeltaTime;
            Rigidbody2D.MovePosition(Rigidbody2D.position + vec_next);
            if (Input.GetMouseButtonDown(1)) { ChargeAttack(); }
        }
        else
        {
            chargeAttackTime += Time.deltaTime;
            if (chargeAttackTime > 0.2) { ChargeAttackEnd(); }
        }
    }

    private void Update()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimPointer.SetMousePosition(mousePos);
    }

    void LateUpdate()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        //���콺 ������ �ٶ󺸵���
        Vector3 aimPointPos = aimPointer.transform.position;
        Vector3 myPosition = transform.position;
        Vector3 direction = (aimPointPos - myPosition).normalized;

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
            Hit(collision.transform.GetComponent<Enemy>().attackPoint * Time.deltaTime);
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

    public void Hit(float damage)
    {
        float realDamage = (damage - defencePower);
        if (realDamage <= 0) { return; }
        hp -= realDamage;
        if (hp <= 0)
        {
            PartsAlphaControl(0f);
            legAnimator.SetTrigger("Dead");
            GameManager.instance.GameOver();
        }
    }

    public void InitializedWeapons()
    {
        foreach (Weapon weapon in equipmentWeaponList)
        {
            Debug.Log(weapon.weaponName + "�ʱ�ȭ �۾���");
            weapon.SwitchWeapon(false);
        }
        Debug.Log(equipmentWeaponList[equipmentWeaponNumber].weaponName + "�ʱ�ȭ �Ϸ��� Ȱ��ȭ");
        equipmentWeaponList[equipmentWeaponNumber].SwitchWeapon(true);
        equipmentWeaponList[equipmentWeaponNumber].LevelUp();
    }

    public bool ChangeStat(ShopItem.InfoType statId, float value)
    {
        switch (statId)
        {
            case ShopItem.InfoType.HP:
                max_hp *= (value+100)/100;
                break;
            case ShopItem.InfoType.HPRecovery:
                hpRecovery *= (value + 100) / 100;
                break;
            case ShopItem.InfoType.AttackPower:
                attackPower *= (value + 100) / 100;
                break;
            case ShopItem.InfoType.Defence:
                defencePower *= (value + 100) / 100;
                break;
            case ShopItem.InfoType.GoldRange:
                if (goldGainRange >= 19) { goldGainRange *= (value + 100) / 100; ; return false; }
                goldGainRange *= (value + 100) / 100;
                break;
            case ShopItem.InfoType.GoldGrid:
                GameManager.instance.goldGrid += (value + 100) / 100;
                break;
            case ShopItem.InfoType.DropGrid:
                GameManager.instance.dropGrid += (value + 100) / 100;
                break;
            case ShopItem.InfoType.Speed:
                if (speed >= 14) { speed += value; return false; }
                speed += value;
                break;
            case ShopItem.InfoType.QuickDraw:
                quickDraw = true;
                return false;
                break;
            case ShopItem.InfoType.BigMagazine:
                bigMagazine = true;
                return false;
                break;
        }
        return true;
    }

    void ChargeAttack()
    {
        //���콺 �������� ����
        Vector3 aimPointPos = aimPointer.transform.position;
        Vector3 myPosition = transform.position;
        Vector3 direction = (aimPointPos - myPosition).normalized;
        Rigidbody2D.velocity = chargeAttackPower * Time.deltaTime * direction;
        //���콺 ������ �ٶ󺸱�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
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
        if (equipmentWeaponNumber == num) { return; }
        equipmentWeaponList[equipmentWeaponNumber].SwitchWeapon(false);
        equipmentWeaponNumber = num;
        equipmentWeaponList[num].SwitchWeapon(true);
    }

    public Weapon GetPlayerWeapon()
    {
        return equipmentWeaponList[equipmentWeaponNumber];
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
