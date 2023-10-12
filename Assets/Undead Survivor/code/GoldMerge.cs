using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMerge : MonoBehaviour
{
    public Gold owner;
    public Gold oldGold;

    private void Awake()
    {
        owner = GetComponentInParent<Gold>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.transform.CompareTag("GoldArea")) { return; }
        if (owner.isCreate) { return; }
        if (owner.isMoving) { return; }
        if (owner.isGem) { return; }

        oldGold = collision.transform.parent.GetComponent<Gold>();
        if (oldGold.isCreate) { return; }
        if (oldGold.isMoving) { return; }
        if (oldGold.isGem) { return; }

        if (oldGold.goldValue >= owner.goldValue) 
        { 
            oldGold.goldValue += owner.goldValue;
            oldGold.SpriteReLoad();
            oldGold = null;
            owner.gameObject.SetActive(false);
        }
        
    }
}
