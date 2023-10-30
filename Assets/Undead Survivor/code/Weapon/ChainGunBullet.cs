using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ChainGunBullet : MonoBehaviour
{
    float shiftRange;
    float damage;
    public float life = 0.5f;
    Vector2 myPostion;
    Vector2 targetPostion;

    List<Transform> parentsList;
    List<Transform> hitList;
    public GameObject prefab;
    public LayerMask target_layer;
    public RaycastHit2D[] targets;
    public Transform myParents;
    public Transform nearest_target;

    LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
    }

    private void FixedUpdate()
    {
        if (nearest_target != null && myParents != null)
        {
            myPostion = nearest_target.position;
            targetPostion = myParents.position;
            Vector3[] line = { myPostion, targetPostion };
            lineRenderer.SetPositions(line);
        }
    }

    public void Init(float inputDamage, int shiftLife, float shiftRan, List<Transform> list, List<Transform> hitlist)
    {
        damage = inputDamage;
        shiftRange = shiftRan;
        parentsList = list.ToList();
        hitList = hitlist.ToList();
        targets = Physics2D.CircleCastAll(transform.position, shiftRange, Vector2.zero, 0, target_layer);
        nearest_target = GetNearest();
        myParents = parentsList[parentsList.Count - 1];
        if (nearest_target != null && myParents != null)
        {
            myPostion = nearest_target.position;
            targetPostion = myParents.position;

            transform.position = myPostion;
            nearest_target.GetComponent<Enemy>().Hit(damage*GameManager.instance.player.attackPower);
            parentsList.Add(transform);
            hitList.Add(nearest_target);
            if (shiftLife > 0) 
            {
                GameObject bullet = Instantiate(prefab, myPostion, Quaternion.identity);
                bullet.GetComponent<ChainGunBullet>().Init(damage, shiftLife - 1, shiftRange, parentsList, hitList);
            }
        }
        Invoke("DestroyObj", life); 
    }

    public Transform GetNearest()
    {
        Transform result = null;
        float diff = 10000;
        RaycastHit2D[] filteredTargets;
        filteredTargets = targets.Where(hit => !hitList.Any(trans => hit.transform == trans)).ToArray();

        targets = filteredTargets.ToArray();
        foreach (RaycastHit2D raycastHit2D in filteredTargets)
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
            
            if (Vector3.Distance(transform.position, result.transform.position) > shiftRange) { result = null; }
        }

        return result;
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }
}
