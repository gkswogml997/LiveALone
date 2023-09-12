using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    public Transform[] spwan_point;
    public SpawnData[] spawnData;

    int level;
    float level_up_time = 20f;
    float timer;

    void Awake()
    {
        spwan_point = GetComponentsInChildren<Transform>();
    }
    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.game_time / level_up_time),spawnData.Length);

        if (timer > spawnData[level].spawn_time)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spwan_point[Random.Range(1, spwan_point.Length)].transform.position;
        enemy.GetComponent<Enemy>().Init(spawnData[level]);
    }
}

[System.Serializable]
public class SpawnData
{
    public int sprite_type;
    public float spawn_time;
    public int hp;
    public float speed;
}
