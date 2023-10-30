using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public GameObject[] titles;
    public Text score;
    public Button retry;

    public void Lose()
    {
        score.text = "Your Score\n" + GameManager.instance.score;
        titles[0].SetActive(true);
        score.gameObject.SetActive(true);
        retry.gameObject.SetActive(true);
    }
}
