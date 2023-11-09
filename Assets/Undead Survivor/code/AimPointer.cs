using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AimPointer : MonoBehaviour
{
    public float AimSpeed = 50;
    float resetTimer = 25;
    public Transform aimingTarget = null;
    bool manualAiming = false;

    Vector2 targetPosition;
    Vector2 mousePos;
    Player player;
    Scanner scanner;
    Vector3 weaponFirePos;
    LineRenderer lineRenderer;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        scanner = player.GetComponent<Scanner>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

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
        if (player == null) { return; }
        if (pos == Vector2.zero) {
            resetTimer -= 1;
            if (resetTimer < 0 && aimingTarget == null)
            {
                mousePos = weaponFirePos; 
            }
            manualAiming = false;
            player.GetPlayerWeapon().isInRange = false;
        }
        else { 
            mousePos = pos;
            player.GetPlayerWeapon().isInRange = true;
            manualAiming = true; 
            resetTimer = 25; 
        }
    }
}
