using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundProps : MonoBehaviour
{
    public Sprite[] sprites;
    public GameObject prefab;
    public int perfabNumber;
    public float createRange;

    public List<GameObject> propsList;

    private void Awake()
    {
        for(int i = 0; i < perfabNumber; i++)
        {
            Vector3 randPostion = new Vector3(Random.Range(-createRange, createRange), Random.Range(-createRange, createRange), 0f);
            GameObject back = Instantiate(prefab, randPostion, Quaternion.identity);
            back.transform.parent = this.transform;
            SpriteRenderer renderer = back.GetComponent<SpriteRenderer>();
            renderer.sprite = sprites[Random.Range(0, sprites.Length)];
            renderer.flipX = (Random.Range(0, 2) == 1);
            propsList.Add(back);
        }
    }
}
