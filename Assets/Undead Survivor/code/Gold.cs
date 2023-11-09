using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;
using Text = UnityEngine.UI.Text;

public class Gold : MonoBehaviour
{
    [Header("��� ����")]
    public int goldValue = 0;
    public int itemId = 0;
    public string nameColor = "white";
    public string itemName = "Undefined Gem";
    public float speed = 0;
    public float detectionRadius = 0; //���� �ݰ�
    public List<Sprite> goldImageList;

    [Header("���� ���� ����")]
    public bool isMoving = false;
    public bool isCreate = true;
    private bool isFalling = false;
    public bool isGem;
    public bool isConfirmedUnique = false;
    private Vector3 startPos;
    public float floatTime = 0.3f;
    public float floatSpeed = 5f;
    public float pushForce = 10f;


    [SerializeField]
    [Header("������Ʈ ����")]
    Player player;
    Text itemInfo;
    Image infoBackground;
    SpriteRenderer spriteRenderer;
    List<GemData> gemData;
    Sprite[] gemSpriteList;

    private void Awake()
    {
        player = GameManager.instance.player;
        Transform textTrans = transform.Find("Canvas/InfoText");
        Transform backTrans = transform.Find("Canvas/Background");
        gemData = GameManager.instance.jsonLoader.gemDataList;
        gemSpriteList = GameManager.instance.jsonLoader.gemSpriteList;

        itemInfo = textTrans.GetComponent<Text>();
        infoBackground = backTrans.GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void FixedUpdate()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        if (isCreate) { CreateAnimation(); }
        else {
            transform.rotation = Quaternion.identity;
            itemInfo.gameObject.SetActive(true);
            infoBackground.gameObject.SetActive(true);
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRadius && !isMoving) { isMoving = true; }
        if (distanceToPlayer > detectionRadius) { isMoving = false; }

        if (isMoving)
        {
            Vector3 dir = (player.transform.position - transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime);

            if (distanceToPlayer < 0.5f)
            {
                Dead();
            }
        }
    }


    public void Init(int value, bool uniqueCheck)
    {
        itemId = 0;
        isGem = false;
        isConfirmedUnique = uniqueCheck;
        //���� ���� �ƴ���
        int randomNumber = Random.Range(1, 101);

        if (randomNumber <= 10 * GameManager.instance.dropGrid/100 || isConfirmedUnique)
        {
            CreateGem();
        }

        //���� ���� �ʱ�ȭ
        speed = GameManager.instance.player.speed + 1;
        detectionRadius = GameManager.instance.player.goldGainRange;

        //���/������ �ʱ�ȭ
        if (!isGem)
        {
            goldValue = value;
            SpriteReLoad();
        }

        //���� �ִϸ��̼� �ʱ�ȭ
        startPos = transform.position;
        isCreate = true;
        isFalling = false;
        floatTime = 0.2f;
        floatSpeed = 10f;

        //�ػ� �ʱ�ȭ 
        ResizeBackgroundToText();

        //�ִϸ��̼� ���� UI ��Ȱ��ȭ
        itemInfo.gameObject.SetActive(false);
        infoBackground.gameObject.SetActive(false);
    }

    public void SpriteReLoad()
    {
        if (goldValue <= 10) { spriteRenderer.sprite = goldImageList[0]; }
        if (goldValue > 10 && goldValue <= 50) { spriteRenderer.sprite = goldImageList[1]; }
        if (goldValue > 50 && goldValue <= 100) { spriteRenderer.sprite = goldImageList[2]; }
        if (goldValue > 100 && goldValue <= 150) { spriteRenderer.sprite = goldImageList[3]; }
        if (goldValue > 150 && goldValue <= 200) { spriteRenderer.sprite = goldImageList[4]; }
        if (goldValue > 200) { spriteRenderer.sprite = goldImageList[5]; }

        itemInfo.text = goldValue.ToString() + " G";
    }

    public void CreateGem()
    {
        isGem = true;
        itemId = Random.Range(0, 9);
        int randomNumber = Random.Range(1, 101);
        if (randomNumber <= 20 || isConfirmedUnique)
        {
            itemId = Random.Range(9, gemData.Count);
        }

        nameColor = gemData[itemId].nameColor;
        itemName = gemData[itemId].textName;
        spriteRenderer.sprite = gemSpriteList[itemId];

        itemInfo.text = "<color="+ nameColor + ">"+itemName+"</color>";
    }

    private void CreateAnimation()
    {
        transform.Rotate(new Vector3(0, 0, 5));
        Vector3 myPos = transform.position;
        if (!isFalling)
        {
            myPos.y += floatSpeed * Time.deltaTime;
            transform.position = myPos;
            floatTime -= Time.deltaTime;
            if (floatTime <= 0) { isFalling = true; }
        }else
        {
            myPos.y -= floatSpeed * Time.deltaTime;
            transform.position = myPos;
            if (transform.position.y < startPos.y) { isCreate = false; }
        }
    }

    private void ResizeBackgroundToText()
    {

        // �ؽ�Ʈ�� ũ�⸦ ������
        float textWidth = itemInfo.preferredWidth;
        float textHeight = itemInfo.preferredHeight;
        float textScaleX = itemInfo.transform.localScale.x;
        float textScaleY = itemInfo.transform.localScale.y;

        // ��� �̹����� RectTransform�� ������
        RectTransform bgRectTransform = infoBackground.rectTransform;

        // ��� �̹����� ũ�� ����
        bgRectTransform.sizeDelta = new Vector2(textWidth * textScaleX, textHeight * textScaleY);
    }

    void Dead()
    {
        if (!isGem)
        { 
            GameManager.instance.gold += (int)(goldValue * GameManager.instance.goldGrid/100);
            goldValue = 0;
        }
        else
        {
            if (gemData[itemId].rating == "unique")
            { GameManager.instance.itemAlert.InsertAlertQueue(spriteRenderer.sprite, itemInfo.text); }
            player.InventoryAdd(itemId, 1);
        }
        gameObject.SetActive(false);
    }
}
