using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health}
    public InfoType type;

    Text myText;
    Slider mySilder;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySilder = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[GameManager.instance.level];
                mySilder.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv. {0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                float remein_time = GameManager.instance.max_game_time - GameManager.instance.game_time;
                int min = Mathf.FloorToInt(remein_time / 60);
                int sec = Mathf.FloorToInt(remein_time % 60);
                myText.text = string.Format("{0:D2} : {1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHp = GameManager.instance.hp;
                float maxHp = GameManager.instance.max_hp;
                mySilder.value = curHp / maxHp;
                break;
        }
    }
}
