using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject prefab; // 프리팹 오브젝트를 연결할 변수
    int numberOfPrefabs = 18; // 생성할 프리팹의 수
    float radius = 25f; // 원의 반지름

    public List<Transform> spwan_point;
    public List<MonsterData> monsterDatas;
    public List<MonsterData> bossDatas;

    float timer;
    float bossTimer;

    void Awake()
    {
        float angleStep = 360f / numberOfPrefabs;

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // 각도를 라디안으로 변환
            float angle = i * angleStep * Mathf.Deg2Rad;

            // 원형으로 배치할 위치 계산
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // 프리팹을 생성하고 위치를 설정하여 배치
            Vector3 spawnPosition = new Vector3(x, y, 0f) + transform.position;
            Quaternion spawnRotation = Quaternion.Euler(0f, 0f, i * angleStep);
            GameObject point = Instantiate(prefab, spawnPosition, spawnRotation);
            point.transform.parent = transform;
            spwan_point.Add(point.transform);
        }

        monsterDatas = GameManager.instance.jsonLoader.monsterDatas;
        bossDatas = GameManager.instance.jsonLoader.bossDatas;
    }
    void Update()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        timer += Time.deltaTime;
        bossTimer += Time.deltaTime;

        if (timer > 1)
        {
            timer = 0f;
            Spawn();
        }

        if (bossTimer > 60)
        {
            bossTimer = 0f;
            BossSpawn();
        }
    }

    void Spawn()
    {
        int randMonster = Random.Range(0, monsterDatas.Count);
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spwan_point[Random.Range(1, spwan_point.Count)].transform.position;
        enemy.GetComponent<Enemy>().isBoss = false;
        enemy.GetComponent<Enemy>().Init(monsterDatas[randMonster]);
    }

    void BossSpawn()
    {
        int randMonster = Random.Range(0, bossDatas.Count);
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spwan_point[Random.Range(1, spwan_point.Count)].transform.position;
        enemy.GetComponent<Enemy>().isBoss = true;
        enemy.GetComponent<Enemy>().Init(bossDatas[randMonster]);
        GameManager.instance.popupAlert.InsertAlertQueue(GameManager.instance.jsonLoader.popUpAlert[1].title, GameManager.instance.jsonLoader.popUpAlert[1].content);
    }
}

