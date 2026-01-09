using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovment : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Transform target; //target 

    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(target == null)
        {
            movement = Vector2.zero;
            return; 
        }
        Vector2 direction = (target.position - transform.position); 
        
        if (direction.magnitude < 0.05)
        {
            movement = Vector2.zero;
        }
        else
        {
            movement = direction.normalized;
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
