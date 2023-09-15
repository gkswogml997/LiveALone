using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# ���� ��Ʈ��")]
    //���� ���̵� ����
    public float game_time;
    public float max_game_time = 40f;

    [Header("# ���� �ڻ�")]
    public int hp;
    public int max_hp = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 20, 30, 40, 50, 60 };

    [Header("# �۷ι� ������Ʈ �����")]
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
