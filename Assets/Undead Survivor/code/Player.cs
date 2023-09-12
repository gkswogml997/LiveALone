using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 vec_input;
    public float speed = 5;
    Rigidbody2D Rigidbody2D;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        vec_input.x = Input.GetAxisRaw("Horizontal");
        vec_input.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 vec_next = vec_input.normalized * speed * Time.fixedDeltaTime;
        Rigidbody2D.MovePosition(Rigidbody2D.position+ vec_next);
    }

    void LateUpdate()
    {
        animator.SetFloat("Speed", vec_input.magnitude);
        if (vec_input.x != 0) 
        { 
            spriteRenderer.flipX = vec_input.x < 0; 
        }
    }
}
