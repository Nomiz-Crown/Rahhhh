using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovment : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform[] WayPoint; //target 

    private int currentIndex = 0; 
    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer sr;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (movement.x != 0)
        {
            sr.flipX = movement.x < 0;
        }
   
        if (currentIndex >= WayPoint.Length)
        {
            movement = Vector2.zero;
            return; 
        }
        Vector2 target = WayPoint[currentIndex].position;
        Vector2 dir = (target - rb.position);
        
        if (dir.magnitude < 0.05)
        {
            currentIndex++;
        }
        else
        {
            movement = dir.normalized;
        }


        //animations
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);
        animator.SetBool("IsMoving", movement != Vector2.zero);
    }

    void FixedUpdate()
    {
        // apply move grej
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
    