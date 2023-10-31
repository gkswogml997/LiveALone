using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# ���� ��Ʈ��")]
    //���� ����
    public int screenWidth;
    public int screenHeight;
    public float game_time;
    public int gameLevel = 1;
    public float levelUpTime;
    public float shopResetTimeMax = 5;
    public float shopResetTIme = 0;
    public string language = "kor";
    public bool showHint = true;
    public bool isLive;
    public bool isGamestart;

    [Header("# ���� �ڻ�")]
    public int score = 0;
    public int kill = 0;
    public int gold = 0;
    public float goldGrid = 1;
    public float dropGrid = 1;

    [Header("# �۷ι� ������Ʈ �����")]
    public Canvas canvas;
    public PoolManager pool;
    public GameObject playerIntro;
    public Player player;
    public Shop shop;
    public Shop blacksmith;
    public Shop inventory;
    public SettingUI settingUI;
    public WeaponChangeUI changeUI;
    public Result uiResult;
    public GameObject enemyCleaner;
    public InventoryItemInfo inventoryItemInfo;
    public PopUpAlert popupAlert;
    public ItemAlert itemAlert;
    public Transform joystick;

    [Header("# ���� ��ũ��Ʈ ����")]
    public JsonDataLoader jsonLoader;

    void Awake()
    {
        instance = this;
        CameraFixed(screenWidth, screenHeight);
        Application.targetFrameRate = 60;
        AlertUIActivateAll();
        jsonLoader = GetComponent<JsonDataLoader>();
        jsonLoader.Init(language);
        shopResetTIme = shopResetTimeMax;
    }

    public void CameraFixed(int width, int height)
    {

        
    }

    public void GameStart()
    {
        //�ӽ�
        isGamestart = true;
        GameResume();
        playerIntro.SetActive(true);
        gameLevel = 1;
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRutine());
    }

    IEnumerator GameOverRutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(2f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        GameStop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.SFX.Lose);
    }

    public void GameVictory()
    {
        StartCoroutine(GameVictoryRutine());
    }

    IEnumerator GameVictoryRutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        //uiResult.Win();
        GameStop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.SFX.Win);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        //��ǲ�ޱ�
        if (Input.GetKeyDown(KeyCode.I))
        {
            OpenInventory();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenShop();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            OpenBlackSmith();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            popupAlert.InsertAlertQueue("�׽�Ʈ", "���̵�� �˶� �׽�Ʈ �Դϴ�.");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllUI();
        }

        if (isGamestart)
        {
            shopResetTIme -= Time.unscaledDeltaTime;
        }
        if (shopResetTIme < 0)
        {
            if(shop.gameObject.activeSelf == false)
            {
                shop.gameObject.SetActive(true);
                shop.ResetItems();
                shop.gameObject.SetActive(false);
            }
            else
            {
                shop.ResetItems();
            }
            if (inventory.gameObject.activeSelf == false)
            {
                inventory.gameObject.SetActive(true);
                inventory.ResetItems();
                inventory.gameObject.SetActive(false);
            }
            else
            {
                inventory.ResetItems();
            }
            popupAlert.InsertAlertQueue(jsonLoader.popUpAlert[2].title, jsonLoader.popUpAlert[2].content);
            shopResetTIme = shopResetTimeMax;
        }

        //exception
        if (!isLive) { return; }

        //act
        game_time += Time.deltaTime;
        if (gameLevel < (int)(game_time / levelUpTime))
        {
            gameLevel++;
            popupAlert.InsertAlertQueue(jsonLoader.popUpAlert[0].title, jsonLoader.popUpAlert[0].content);
        }
    }

    public void OpenInventory()
    {
        if (!IsUIOpen()) {
            inventory.gameObject.SetActive(true);
            inventory.Show();
        }
        else if (inventory.isOpen) {
            inventory.Hide();
            inventory.gameObject.SetActive(false);
        }
    }
    public void OpenShop()
    {
        if (!IsUIOpen())
        {
            shop.gameObject.SetActive(true);
            shop.Show();
        }
        else if (shop.isOpen)
        {
            shop.Hide();
            shop.gameObject.SetActive(false);
        }
    }
    public void OpenBlackSmith() 
    {
        if (!IsUIOpen())
        {
            blacksmith.gameObject.SetActive(true);
            blacksmith.Show();
        }
        else if (blacksmith.isOpen)
        {
            blacksmith.Hide();
            blacksmith.gameObject.SetActive(false);
        }
    }
    public void OpenSettingUI()
    {
        if (!IsUIOpen())
        {
            settingUI.gameObject.SetActive(true);
            settingUI.Show();
        }
        else if (settingUI.isOpen)
        {
            settingUI.Hide();
            settingUI.gameObject.SetActive(false);
        }
    }
    public void OpenWeaponChangeUI()
    {
        if (!IsUIOpen())
        {
            changeUI.gameObject.SetActive(true);
            changeUI.Show();
        }
        else if (changeUI.isOpen)
        {
            changeUI.Hide();
            changeUI.gameObject.SetActive(false);
        }
    }

    public bool IsUIOpen()
    {
        if (inventory.isOpen) { return true; }
        if (shop.isOpen) { return true; }
        if (blacksmith.isOpen) { return true; }
        if (settingUI.isOpen) { return true; }
        if (changeUI.isOpen) { return true; }
        return false;
    }

    public void CloseAllUI()
    {
        if (inventory.isOpen) {
            inventory.Hide();
            inventory.gameObject.SetActive(false);
        }
        if (shop.isOpen) {
            shop.Hide();
            shop.gameObject.SetActive(false);
        }
        if (blacksmith.isOpen) {
            blacksmith.Hide();
            blacksmith.gameObject.SetActive(false);
        }
        if (changeUI.isOpen)
        {
            changeUI.Hide();
            changeUI.gameObject.SetActive(false);
        }
    }

    public void AlertUIActivateAll()
    {
        popupAlert.gameObject.SetActive(true);
        itemAlert.gameObject.SetActive(true);
    }

    public void GameStop()
    {
        isLive = false;
        Time.timeScale = 0;
        joystick.localScale = Vector3.zero;
    }

    public void GameResume()
    {
        if (!isGamestart) { return; }
        isLive = true;
        Time.timeScale = 1;
        joystick.localScale = Vector3.one * 2.5f;
    }
}
