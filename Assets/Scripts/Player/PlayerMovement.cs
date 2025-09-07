using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f;
    
    private Vector2 movement;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
	private Animator animator;
    private Vector2 targetVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;
		animator = GetComponent<Animator>();
		if (animator == null) Debug.Assert(false, "Animator component missing from player.");
    }
    
    void Update()
    {
        GetInput();
    }
    
    void FixedUpdate()
    {
        MovePlayer();
    }
    
    void GetInput()
    {

		if(Input.GetKey(KeyCode.A)) 
			animator.SetInteger("Direction", 1);
		else if(Input.GetKey(KeyCode.W))
			animator.SetInteger("Direction", 2);
		else if(Input.GetKey(KeyCode.D))
			animator.SetInteger("Direction", 3);
		else if(Input.GetKey(KeyCode.S))
			animator.SetInteger("Direction", 4);
		else
			animator.SetInteger("Direction", 0);

		movement.x = Input.GetAxisRaw("Horizontal");
		movement.y = Input.GetAxisRaw("Vertical");
		movement = movement.normalized;
    }
    
    void MovePlayer()
    {
        if (rb == null) return;
        var speed = moveSpeed * (PlayerStats.speed_modifier / 100);
        targetVelocity = movement * speed;
        rb.linearVelocity = targetVelocity;
    }
}
