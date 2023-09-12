using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_pool_manager : MonoBehaviour
{
    public GameObject[] gameObjects;

    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[gameObjects.Length];
        for(int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int i)
    {
        GameObject select = null;
        foreach(GameObject obj in pools[i])
        {
            if (!obj.activeSelf)
            {
                select = obj;
                select.SetActive(true);
                break;
            }
        }
        if (select == null)
        {
            select = Instantiate(gameObjects[i], transform);
            pools[i].Add(select);
        }

        return select;
    }
}
