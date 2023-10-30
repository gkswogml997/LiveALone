using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    List<InventoryItem> inventory;
    PlayerInventorySlot[] inventorySlots;
    List<GemData> itemData;
    Sprite[] itemSpriteList;


    private void Awake()
    {
        inventory = GameManager.instance.player.inventory;
        inventorySlots = GetComponentsInChildren<PlayerInventorySlot>();
        itemData = GameManager.instance.jsonLoader.gemDataList;
        itemSpriteList = GameManager.instance.jsonLoader.gemSpriteList;
    }

    public void UpToDateList()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.Count)
            { inventorySlots[i].Init(itemSpriteList[inventory[i].itemId], inventory[i].quantity, itemData[inventory[i].itemId].textName, itemData[inventory[i].itemId].textDesc); }
            else { inventorySlots[i].Init(null, 0, null, null); }
            
        }
    }
}
