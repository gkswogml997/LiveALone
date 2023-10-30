using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemAlert : MonoBehaviour
{
    public CanvasRenderer[] objRenderer;
    public float fadeInDuration = 0.1f;
    public float fadeOutDuration = 0.1f;
    public float delayBeforeFadeOut = 0.5f;


    private class ItemData
    {
        public ItemData(Sprite spr, string name)
        {
            this.spr = spr;
            this.name = name;
        }
        public Sprite spr;
        public string name;
    }

    bool showing = false;
    private Queue<ItemData> alertQueue;
    Image itemSprite;
    Text itemName;

    void Awake()
    {
        alertQueue = new Queue<ItemData>();
        objRenderer = GetComponentsInChildren<CanvasRenderer>();
        itemSprite = GetComponentsInChildren<Image>()[1];
        itemName = GetComponentsInChildren<Text>()[1];
        SetAlpha(0f);
    }

    private void FixedUpdate()
    {
        if (alertQueue.Count == 0) { return; }
        if (alertQueue.Count > 0 && !showing)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void InsertAlertQueue(Sprite spr, string name)
    {
        alertQueue.Enqueue(new ItemData(spr,name));
    }


    private void SetAlpha(float alpha)
    {
        foreach (CanvasRenderer renderer in objRenderer)
        { 
            Color color = renderer.GetColor();
            color.a = alpha;
            renderer.SetColor(color);
        }
    }

    private IEnumerator FadeIn()
    {
        showing = true;
        ItemData item = alertQueue.Dequeue();
        itemSprite.sprite = item.spr;
        itemName.text = item.name;
        float currentTime = 0f;
        while (currentTime < fadeInDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeInDuration);
            SetAlpha(alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        // 대기 후에 투명해지도록 코루틴 시작
        yield return new WaitForSeconds(delayBeforeFadeOut);
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float currentTime = 0f;
        while (currentTime < fadeOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeOutDuration);
            SetAlpha(alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        showing = false;
    }
}
