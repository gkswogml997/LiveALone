using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject prefab; // ������ ������Ʈ�� ������ ����
    int numberOfPrefabs = 18; // ������ �������� ��
    float radius = 25f; // ���� ������

    public List<Transform> spwan_point;
    public SpawnData[] spawnData;

    float timer;

    void Awake()
    {
        float angleStep = 360f / numberOfPrefabs;

        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // ������ �������� ��ȯ
            float angle = i * angleStep * Mathf.Deg2Rad;

            // �������� ��ġ�� ��ġ ���
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            // �������� �����ϰ� ��ġ�� �����Ͽ� ��ġ
            Vector3 spawnPosition = new Vector3(x, y, 0f) + transform.position;
            Quaternion spawnRotation = Quaternion.Euler(0f, 0f, i * angleStep);
            GameObject point = Instantiate(prefab, spawnPosition, spawnRotation);
            point.transform.parent = transform;
            spwan_point.Add(point.transform);
        }
    }
    void Update()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        //act
        timer += Time.deltaTime;

        if (timer > 0.25)
        {
            timer = 0f;
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.pool.Get(0);
        enemy.transform.position = spwan_point[Random.Range(1, spwan_point.Count)].transform.position;
        enemy.GetComponent<Enemy>().Init(spawnData[0]);
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
