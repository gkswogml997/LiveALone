using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scan_range;
    public LayerMask target_layer;
    public RaycastHit2D[] targets;
    public Transform nearest_target;

    private void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scan_range, Vector2.zero, 0, target_layer);
        nearest_target = GetNearest();
    }

    public Transform GetNearest()
    {
        Transform result = null;
        float diff = 10000;

        foreach(RaycastHit2D raycastHit2D in targets)
        {
            Vector3 my_pos = transform.position;
            Vector3 target_pos = raycastHit2D.transform.position;
            float curent_diff = Vector3.Distance(my_pos, target_pos);
            if (curent_diff < diff)
            {
                diff = curent_diff;
                result = raycastHit2D.transform;
            }
        }

        if (result != null) 
        { 
            if (!result.CompareTag("Enemy")) { result = null; } 
            if (Vector3.Distance(transform.position,result.transform.position) > scan_range) { result = null; }
        }
        
        return result;
    }
}
