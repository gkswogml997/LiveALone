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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area")) {return;}

        Vector3 player_posision = GameManager.instance.player.transform.position;
        Vector3 my_posision = gameObject.transform.position;
        float diffx = Mathf.Abs(player_posision.x - my_posision.x);
        float diffy = Mathf.Abs(player_posision.y - my_posision.y);

        Vector3 player_dir = GameManager.instance.player.vec_input;
        float dirx = player_dir.x < 0 ? -1 : 1;
        float diry = player_dir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffx > diffy)
                {
                    transform.Translate(Vector3.right * dirx * 40);
                }
                else if (diffx < diffy)
                {
                    transform.Translate(Vector3.up * diry * 40);
                }
                break;
            case "Enemy":
                if (collider.enabled)
                {
                    transform.Translate(player_dir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f),0f));
                }
                break;
        }
    }
}
