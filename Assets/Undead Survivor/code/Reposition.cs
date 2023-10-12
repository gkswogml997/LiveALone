using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reposition_area : MonoBehaviour
{
    Collider2D collider;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) { return; }

        Vector3 player_posision = GameManager.instance.player.transform.position;
        Vector3 my_posision = transform.position;
        
        switch (transform.tag)
        {
            case "Ground":
                float diffx = player_posision.x - my_posision.x;
                float diffy = player_posision.y - my_posision.y;

                float dirx = diffx < 0 ? -1 : 1;
                float diry = diffy < 0 ? -1 : 1;
                diffx = Mathf.Abs(diffx);
                diffy = Mathf.Abs(diffy);

                if (diffx > diffy)
                {
                    transform.Translate(Vector3.right * dirx * 160);
                }
                else if (diffx < diffy)
                {
                    transform.Translate(Vector3.up * diry * 160);
                }
                else
                {
                    transform.Translate(dirx * 160, diry * 160, 0);
                }
                break;
            case "Enemy":
                if (collider.enabled)
                {
                    Vector3 dist = player_posision - my_posision;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
            case "BackgroundProps":
                float diffx2 = player_posision.x - my_posision.x;
                float diffy2 = player_posision.y - my_posision.y;

                float dirx2 = diffx2 < 0 ? -1 : 1;
                float diry2 = diffy2 < 0 ? -1 : 1;
                diffx2 = Mathf.Abs(diffx2);
                diffy2 = Mathf.Abs(diffy2);

                if (diffx2 > diffy2)
                {
                    transform.Translate(Vector3.right * dirx2 * 80);
                }
                else if (diffx2 < diffy2)
                {
                    transform.Translate(Vector3.up * diry2 * 80);
                }
                else
                {
                    transform.Translate(dirx2 * 80, diry2 * 80, 0);
                }
                break;
        }
    }
}
