using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AimPointer : MonoBehaviour
{
    public float AimSpeed = 50;
    public Transform aimingTarget = null;
    bool manualAiming = false;

    Vector2 targetPosition;
    Vector2 mousePos;
    Camera cam;
    Player player;
    Scanner scanner;
    Vector3 weaponFirePos;
    LineRenderer lineRenderer;

    private void Awake()
    {
        cam = Camera.main;
        player = GetComponentInParent<Player>();
        scanner = player.GetComponent<Scanner>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

    }

    private void FixedUpdate()
    {
        /*mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0)) { 
            manualAiming = true;
            targetPosition = mousePos;
        }
        else { manualAiming = false; }*/
        
    }

    private void Update()
    {
        if (manualAiming)
        {
            targetPosition = mousePos;
        }
        else
        {
            aimingTarget = scanner.nearest_target;
            if (aimingTarget != null && aimingTarget.CompareTag("Enemy"))
            {
                targetPosition = aimingTarget.position;
            }
            else
            {
                targetPosition = mousePos;
            }
        }

        weaponFirePos = player.GetPlayerWeapon().transform.GetChild(0).transform.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, AimSpeed * Time.deltaTime);

        Vector3[] pos = { transform.position, weaponFirePos };
        lineRenderer.SetPositions(pos);

    }
    
    public void SetMousePosition(Vector2 pos)
    {
        mousePos = pos;
        Debug.Log("마우스 위치 전달받음");
    }
}
