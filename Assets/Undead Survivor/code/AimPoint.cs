using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPoint : MonoBehaviour
{
    private void Update()
    {
        Vector3 mouseOriPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Input.mousePosition.z);
        transform.position = Camera.main.ScreenToWorldPoint(mouseOriPos);
    }
}
