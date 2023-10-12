using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.SearchService;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# 게임 컨트롤")]
    //게임 설정
    public float game_time;
    public string language = "kor";
    public bool isLive;

    [Header("# 게임 자산")]
    public int kill = 0;
    public int gold = 0;

    [Header("# 글로벌 오브젝트 연결용")]
    public Canvas canvas;
    public PoolManager pool;
    public Player player;
    public Shop shop;
    public Shop blacksmith;
    public Inventory inventory;
    public Result uiResult;
    public GameObject enemyCleaner;

    [Header("# 내부 스크립트 연동")]
    public JsonDataLoader jsonLoader;

    void Awake()
    {
        instance = this;
        jsonLoader = GetComponent<JsonDataLoader>();
        jsonLoader.Init(language);
    }

    public void GameStart()
    {
        //임시
        GameResume();
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.SFX.Select);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRutine());
    }

    IEnumerator GameOverRutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

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
        uiResult.Win();
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
        //인풋받기
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseAllUI();
        }

        //exception
        if (!isLive) { return; }

        //act
        game_time += Time.deltaTime;

    }
    public void OpenInventory()
    {
        if (!inventory.isOpen) { inventory.Show(); }
        else if (inventory.isOpen) { inventory.Hide(); }
    }
    public void OpenShop()
    {
        if (!shop.isOpen && !blacksmith.isOpen) { shop.Show(); }
        else if (shop.isOpen) { shop.Hide(); }
    }
    public void OpenBlackSmith() 
    {
        if (!shop.isOpen && !blacksmith.isOpen) { blacksmith.Show(); }
        else if (blacksmith.isOpen) { blacksmith.Hide(); }
    }

    public void CloseAllUI()
    {
        if (inventory.isOpen) { inventory.Hide(); }
        if (shop.isOpen) { shop.Hide(); }
        if (blacksmith.isOpen) { blacksmith.Hide(); }
    }

    public void GameStop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void GameResume()
    {
        isLive = true;
        Time.timeScale = 1;
    }
}
