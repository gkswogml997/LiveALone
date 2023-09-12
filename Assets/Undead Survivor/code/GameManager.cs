using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //게임 난이도 설정
    public float game_time;
    public float max_game_time = 40f;

    public Enemy_pool_manager pool;
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
}
