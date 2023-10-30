using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using static UnityEditor.PlayerSettings;
#endif

public class LookAtMouse : MonoBehaviour
{
    public float myPosX;
    public float myPosY;
    public float rotationSpeed = 10f;

    Vector2 myPos ,myPosReverse;

    Player player;
    GameObject weaponParent;
    SpriteRenderer spriteRenderer;
    SpriteRenderer[] weaponSpriteRenderer;

    private void Awake()
    {
        myPos = new Vector2(myPosX, myPosY);
        myPosReverse = new Vector2(-myPosX, myPosY);
        player = GetComponentInParent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponParent = transform.GetChild(0).gameObject;
        weaponSpriteRenderer = weaponParent.GetComponentsInChildren<SpriteRenderer>(true);
    }

    private void FixedUpdate()
    {
        //exception
        if (!GameManager.instance.isLive) { return; }

        Vector3 aimPointPos = player.aimPointer.transform.position;
        Vector3 gunPosition = transform.position; // ÃÑ±¸ÀÇ À§Ä¡
        Vector3 direction = (aimPointPos - gunPosition).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float normalizedAngle = (angle + 360) % 360;

        if (normalizedAngle >= 90 && normalizedAngle < 270)
        {
            transform.localPosition = myPos;
            foreach(SpriteRenderer sprite in weaponSpriteRenderer) {sprite.flipY = true;}
            spriteRenderer.flipY = true;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = myPosReverse;
            foreach (SpriteRenderer sprite in weaponSpriteRenderer) { sprite.flipY = false; }
            spriteRenderer.flipY = false;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        

    }
}
