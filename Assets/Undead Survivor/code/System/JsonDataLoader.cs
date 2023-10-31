using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*상점 아이템 섹션*/
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
    public List<ShopItemData> sellItems;
}
/*다이어 로그 섹션*/
[System.Serializable]
public class ShopDialog
{
    public string shopKeeperName;
    public string sayHi;
    public string buy;
    public string moreGold;
    public string fail;
}
[System.Serializable]
public class InventoryDialog
{
    public string shopKeeperName;
    public string sayHi;
    public string buy;
    public string buy2;
    public string buy3;
    public string fail;
}
[System.Serializable]
public class ShopDialogDatas
{
    public ShopDialog shop;
    public ShopDialog blackSmith;
    public InventoryDialog inventory;
}

/*드랍 아이템 섹션*/
[System.Serializable]
public class GemData
{
    public int gemId;
    public string nameColor;
    public string textName;
    public string textDesc;
    public string rating;
}
[System.Serializable]
public class DropItemDatabase
{
    public List<GemData> gems;
}

/*무기 레벨업 데이터 섹션*/
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

/*LaserGun*/
[System.Serializable]
public class LaserGunLevelDecs
{
    public string weaponName;
    public string weaponDesc;
    public string damage;
    public string speed;
    public string laserWidth;
}
[System.Serializable]
public class LaserGunLevelUpData
{
    public int level;
    public float damageRise;
    public float speedRise;
    public float laserWidth;
}
[System.Serializable]
public class LaserGunLevelUpDataList
{
    public List<LaserGunLevelUpData> levelDatas;
}

/*ChainGun*/
[System.Serializable]
public class ChainGunLevelDecs
{
    public string weaponName;
    public string weaponDesc;
    public string damage;
    public string speed;
    public string targetNumber;
    public string targetDir;
}
[System.Serializable]
public class ChainGunLevelUpData
{
    public int level;
    public float damageRise;
    public float speedRise;
    public int targetNumber;
    public int targetDir;
}
[System.Serializable]
public class ChainGunLevelUpDataList
{
    public List<ChainGunLevelUpData> levelDatas;
}

/*FireGun*/
[System.Serializable]
public class FireGunLevelDecs
{
    public string weaponName;
    public string weaponDesc;
    public string damage;
    public string range;
    public string dotDamage;
}
[System.Serializable]
public class FireGunLevelUpData
{
    public int level;
    public float damageRise;
    public float range;
    public float dotDamage;
}
[System.Serializable]
public class FireGunLevelUpDataList
{
    public List<FireGunLevelUpData> levelDatas;
}

/*ShotGun*/
[System.Serializable]
public class ShotGunLevelDecs
{
    public string weaponName;
    public string weaponDesc;
    public string damage;
    public string range;
    public string bulletNumber;
}
[System.Serializable]
public class ShotGunLevelUpData
{
    public int level;
    public float damageRise;
    public float range;
    public float bulletNumber;
}
[System.Serializable]
public class ShotGunLevelUpDataList
{
    public List<ShotGunLevelUpData> levelDatas;
}

/*무기 전체 string 부르기*/
[System.Serializable]
public class WeaponLevelUpDesc
{
    public List<PistolLevelDecs> pistol;
    public List<Ak47LevelDecs> ak47;
    public List<LaserGunLevelDecs> laserGun;
    public List<ChainGunLevelDecs> chainGun;
    public List<FireGunLevelDecs> fireGun;
    public List<ShotGunLevelDecs> shotGun;
}

/*몬스터 데이터 섹션*/
[System.Serializable]
public class MonsterData
{
    public int monsterNumber;
    public float hp;
    public float speed;
    public float range;
    public float attackPoint;
    public float attackSpeed;
}
[System.Serializable]
public class MonsterDataList
{
    public List<MonsterData> monsterDatas;
    public List<MonsterData> bossDatas;
}
[System.Serializable]
public class AlertData
{
    public string title;
    public string content;
}
[System.Serializable]
public class AlertDataList
{
    public List<AlertData> popUp;
    public List<AlertData> hint;
}

public class JsonDataLoader : MonoBehaviour
{
    [Header("수동 추가!!!")]
    public Sprite[] gemSpriteList;
    public Sprite[] shopItemSpriteList;
    public Sprite[] shopSellItemSpriteList;

    [Header("상점 아이템")]
    public List<ShopItemData> shopItemDataList;
    public List<ShopItemData> shopSellItemDataList;
    public List<GemData> gemDataList;
    public ShopDialog shopDialog;
    public ShopDialog blacksmithDialog;
    public InventoryDialog inventoryDialog;

    [Header("무기 레벨업 데이터")]
    public List<Ak47LevelUpData> Ak47_levelUpDataList;
    public Ak47LevelDecs Ak47_levelUpDesc;
    public List<PistolLevelUpData> Pistol_levelUpDataList;
    public PistolLevelDecs Pistol_levelUpDesc;
    public List<LaserGunLevelUpData> LaserGun_levelUpDataList;
    public LaserGunLevelDecs LaserGun_levelUpDesc;
    public List<ChainGunLevelUpData> ChainGun_levelUpDataList;
    public ChainGunLevelDecs ChainGun_levelUpDesc;
    public List<FireGunLevelUpData> FireGun_levelUpDataList;
    public FireGunLevelDecs FireGun_levelUpDesc;
    public List<ShotGunLevelUpData> ShotGun_levelUpDataList;
    public ShotGunLevelDecs ShotGun_levelUpDesc;

    [Header("알람 데이터")]
    public List<AlertData> popUpAlert;
    public List<AlertData> hintAlert;

    [Header("몬스터 데이터")]
    public List<MonsterData> monsterDatas;
    public List<MonsterData> bossDatas;

    public void Init(string language)
    {
        LoadShopItemData(language);
        LoadDropItemData(language);
        LoadShopDialog(language);
        LoadWeaponLevelUpDesc(language);
        LoadAlert(language);
        LoadPistolLevelUpData();
        LoadAk47LevelUpData();
        LoadLaserGunLevelUpData();
        LoadChainGunLevelUpData();
        LoadFireGunLevelUpData();
        LoadShotGunLevelUpData();
        LoadMonsterData();
    }

    public void LoadShopItemData(string language)
    {
        string filePath = "localization/ShopItemData_" + language;
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        ItemDatabase list = JsonUtility.FromJson<ItemDatabase>(jsonContent);
        shopItemDataList = list.items;
        shopSellItemDataList = list.sellItems;
    }
    public void LoadDropItemData(string language)
    {
        string filePath = "localization/DropItemData_" + language;
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        DropItemDatabase list = JsonUtility.FromJson<DropItemDatabase>(jsonContent);
        gemDataList = list.gems;
    }
    public void LoadShopDialog(string language)
    {
        string filePath = "localization/ShopDialog_" + language;
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        ShopDialogDatas datas = JsonUtility.FromJson<ShopDialogDatas>(jsonContent);
        shopDialog = datas.shop;
        blacksmithDialog = datas.blackSmith;
        inventoryDialog = datas.inventory;
    }
    public void LoadWeaponLevelUpDesc(string language)
    {
        string filePath = "localization/WeaponLevelUpDesc_" + language;
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        WeaponLevelUpDesc list = JsonUtility.FromJson<WeaponLevelUpDesc>(jsonContent);
        Pistol_levelUpDesc = list.pistol[0];
        Ak47_levelUpDesc = list.ak47[0];
        LaserGun_levelUpDesc = list.laserGun[0];
        ChainGun_levelUpDesc = list.chainGun[0];
        FireGun_levelUpDesc = list.fireGun[0];
        ShotGun_levelUpDesc = list.shotGun[0];
    }
    public void LoadAlert(string language)
    {
        string filePath = "localization/Alert_" + language;
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        AlertDataList datas = JsonUtility.FromJson<AlertDataList>(jsonContent);
        popUpAlert = datas.popUp;
        hintAlert = datas.hint;
    }

    //현지화가 필요 없는 부분
    public void LoadPistolLevelUpData()
    {
        string filePath = "weaponData/PistolLevelUpData";
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        PistolLevelUpDataList list = JsonUtility.FromJson<PistolLevelUpDataList>(jsonContent);
        Pistol_levelUpDataList = list.levelDatas;
    }
    public void LoadAk47LevelUpData()
    {
        string filePath = "weaponData/Ak47LevelUpData";
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        Ak47LevelUpDataList list = JsonUtility.FromJson<Ak47LevelUpDataList>(jsonContent);
        Ak47_levelUpDataList = list.levelDatas;
    }
    public void LoadLaserGunLevelUpData()
    {
        string filePath = "weaponData/LaserGunLevelUpData";
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        LaserGunLevelUpDataList list = JsonUtility.FromJson<LaserGunLevelUpDataList>(jsonContent);
        LaserGun_levelUpDataList = list.levelDatas;
    }
    public void LoadChainGunLevelUpData()
    {
        string filePath = "weaponData/ChainGunLevelUpData";
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        ChainGunLevelUpDataList list = JsonUtility.FromJson<ChainGunLevelUpDataList>(jsonContent);
        ChainGun_levelUpDataList = list.levelDatas;
    }
    public void LoadFireGunLevelUpData()
    {
        string filePath = "weaponData/FireGunLevelUpData";
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        FireGunLevelUpDataList list = JsonUtility.FromJson<FireGunLevelUpDataList>(jsonContent);
        FireGun_levelUpDataList = list.levelDatas;
    }
    public void LoadShotGunLevelUpData()
    {
        string filePath = "weaponData/ShotGunLevelUpData";
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        ShotGunLevelUpDataList list = JsonUtility.FromJson<ShotGunLevelUpDataList>(jsonContent);
        ShotGun_levelUpDataList = list.levelDatas;
    }
    public void LoadMonsterData()
    {
        string filePath = "MonsterData/MonsterData";
        string jsonContent = Resources.Load<TextAsset>(filePath).ToString();
        MonsterDataList list = JsonUtility.FromJson<MonsterDataList>(jsonContent);
        monsterDatas = list.monsterDatas;
        bossDatas = list.bossDatas;
    }
}
