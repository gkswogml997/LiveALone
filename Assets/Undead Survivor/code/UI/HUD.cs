using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health, Weapon, Gold}
    public InfoType type;

    public Player player;

    Text myText;
    Slider mySilder;

    private void Awake()
    {
        player = GameManager.instance.player;
        myText = GetComponent<Text>();
        mySilder = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                break;
            case InfoType.Gold:
                myText.text = GameManager.instance.gold+"G";
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.score);
                break;
            case InfoType.Time:
                float remein_time = GameManager.instance.game_time;
                int min = Mathf.FloorToInt(remein_time / 60);
                int sec = Mathf.FloorToInt(remein_time % 60);
                myText.text = string.Format("{0:D2} : {1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHp = player.hp;
                float maxHp = player.max_hp;
                GetComponentsInChildren<Text>()[0].text = (int)curHp+" / "+ (int)maxHp;
                mySilder.value = curHp / maxHp;
                break;
            case InfoType.Weapon:
                break;
        }
    }
}
