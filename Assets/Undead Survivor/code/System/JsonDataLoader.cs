using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

/*���� ������ ����*/
[System.Serializable]
public class ShopItemData
{
    public int itemId;
    public string textName;
    public string textDesc;
}
[System.Serializable]
public class ItemDatabase
{
    public List<ShopItemData> items;
}

/*��� ������ ����*/
[System.Serializable]
public class GemData
{
    public int gemId;
    public string nameColor;
    public string textName;
    public string textDesc;
}
[System.Serializable]
public class DropItemDatabase
{
    public List<GemData> gems;
}

/*���� ������ ������ ����*/
/*pistol*/
[System.Serializable]
public class PistolLevelDecs
{
    public string weaponName;
    public string weaponDesc;
    public string damage;
    public string speed;
}
[System.Serializable]
public class PistolLevelUpData
{
    public int level;
    public float damageRise;
    public float speedRise;
}
[System.Serializable]
public class PistolLevelUpDataList
{
    public List<PistolLevelUpData> levelDatas;
}

/*Ak47*/
[System.Serializable]
public class Ak47LevelDecs
{
    public string weaponName;
    public string weaponDesc;
    public string damage;
    public string per;
    public string speed;
    public string fireRate;
    public string fireCount;
}
[System.Serializable]
public class Ak47LevelUpData
{
    public int level;
    public float damageRise;
    public int perRise;
    public float speedRise;
    public float fireRateRise;
    public int fireCountRise;
}
[System.Serializable]
public class Ak47LevelUpDataList
{
    public List<Ak47LevelUpData> levelDatas;
}


/*���� ��ü string �θ���*/
public class WeaponLevelUpDesc
{
    public List<PistolLevelDecs> pistol;
    public List<Ak47LevelDecs> ak47;
}


public class JsonDataLoader : MonoBehaviour
{
    [Header("���� �߰�!!!")]
    public Sprite[] gemSpriteList;
    public Sprite[] shopItemSpriteList;

    [Header("���� ������")]
    public List<ShopItemData> shopItemDataList;
    public List<GemData> gemDataList;

    [Header("���� ������ ������")]
    public List<Ak47LevelUpData> Ak47_levelUpDataList;
    public Ak47LevelDecs Ak47_levelUpDesc;
    public List<PistolLevelUpData> Pistol_levelUpDataList;
    public PistolLevelDecs Pistol_levelUpDesc;

    public void Init(string language)
    {
        LoadShopItemData(language);
        LoadDropItemData(language);
        LoadWeaponLevelUpDesc(language);
        LoadPistolLevelUpData();
        LoadAk47LevelUpData();
    }

    public void LoadShopItemData(string language)
    {
        string filePath = Application.dataPath + "/Resources/localization/ShopItemData_" + language + ".json";
        string jsonContent = File.ReadAllText(filePath);
        ItemDatabase list = JsonUtility.FromJson<ItemDatabase>(jsonContent);
        shopItemDataList = list.items;
    }
    public void LoadDropItemData(string language)
    {
        string filePath = Application.dataPath + "/Resources/localization/DropItemData_" + language + ".json";
        string jsonContent = File.ReadAllText(filePath);
        DropItemDatabase list = JsonUtility.FromJson<DropItemDatabase>(jsonContent);
        gemDataList = list.gems;
    }
    public void LoadWeaponLevelUpDesc(string language)
    {
        string filePath = Application.dataPath + "/Resources/localization/WeaponLevelUpDesc_" + language + ".json";
        string jsonContent = File.ReadAllText(filePath);
        WeaponLevelUpDesc list = JsonUtility.FromJson<WeaponLevelUpDesc>(jsonContent);
        Pistol_levelUpDesc = list.pistol[0];
        Ak47_levelUpDesc = list.ak47[0];
    }


    //����ȭ�� �ʿ� ���� �κ�
    public void LoadPistolLevelUpData()
    {
        string filePath = Application.dataPath + "/Resources/weaponData/PistolLevelUpData.json";
        string jsonContent = File.ReadAllText(filePath);
        PistolLevelUpDataList list = JsonUtility.FromJson<PistolLevelUpDataList>(jsonContent);
        Pistol_levelUpDataList = list.levelDatas;
    }
    public void LoadAk47LevelUpData()
    {
        string filePath = Application.dataPath + "/Resources/weaponData/Ak47LevelUpData.json";
        string jsonContent = File.ReadAllText(filePath);
        Ak47LevelUpDataList list = JsonUtility.FromJson<Ak47LevelUpDataList>(jsonContent);
        Ak47_levelUpDataList = list.levelDatas;
    }
}
