using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    Animator animatior;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animatior = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animatior.enabled = false;
        spriteRenderer.enabled = false;
    }

    public void StartFireEffect()
    {
        animatior.enabled = true;
        spriteRenderer.enabled = true;
    }
    public void EndFireEffect()
    {
        animatior.enabled = false;
        spriteRenderer.enabled = false;
    }
}
