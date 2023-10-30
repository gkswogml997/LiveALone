using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewPostionController : MonoBehaviour
{
    public Vector3 LastOpenPostion;
    private void Awake()
    {
        LastOpenPostion = new Vector3(0f, 0f, 0f);
    }

    public void SavePostion()
    {
        LastOpenPostion = transform.position;
    }

    public void MovePostion()
    {
        transform.position = LastOpenPostion;
    }
}
