using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Alert
{
    public Alert(string title, string content){
        this.title = title;
        this.content = content;
    }
    public string title;
    public string content;
}

public class PopUpAlert : MonoBehaviour
{
    public float popupSpeed = 20;
    bool showing = false;
    Queue<Alert> alertQueue;
    Text titleText;
    Text contentText;

    private void Awake()
    {
        titleText = GetComponentsInChildren<Text>()[0];
        contentText = GetComponentsInChildren<Text>()[1];
        alertQueue = new Queue<Alert>();
    }

    private void FixedUpdate()
    {
        if(alertQueue.Count > 0 && !showing)
        {
            StartCoroutine(ShowAlert());
        }
    }

    public void InsertAlertQueue(string title, string content)
    {
        Alert a = new Alert(title, content);
        alertQueue.Enqueue(a);
    }

    IEnumerator ShowAlert()
    {
        showing = true;
        Alert a = alertQueue.Dequeue();
        titleText.text = a.title;
        contentText.text = a.content;
        while(transform.position.x < 10)
        {
            transform.position += Vector3.right * popupSpeed;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(3f);
        while (transform.position.x > -400)
        {
            transform.position += Vector3.left * popupSpeed;
            yield return new WaitForFixedUpdate();
        }
        showing = false;
    }
}

