using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# 게임 컨트롤")]
    //게임 난이도 설정
    public float game_time;
    public float max_game_time = 40f;

    [Header("# 게임 자산")]
    public int hp;
    public int max_hp = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 20, 30, 40, 50, 60 };

    [Header("# 글로벌 오브젝트 연결용")]
    public PoolManager pool;
    public Player player;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        game_time += Time.deltaTime;
        
        if (game_time > max_game_time)
        {
             game_time = max_game_time;
        }
    }

    public void GetExp()
    {
        exp++;
        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
