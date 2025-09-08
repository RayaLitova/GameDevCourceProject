using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer sprite_renderer;
    protected Vector2 roam_direction;
    protected float roam_change_time = 0;
    protected Rigidbody2D rb;
    protected Vector2 targetVelocity;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Roam();
    }

    public void Roam()
    {
        if(roam_change_time < Time.time) 
        {
			if(Random.Range(0, 100) > 30) 
        	{
        	    if(animator.GetBool("isMoving"))
        	        animator.SetBool("isMoving", false); 
        	    roam_direction = Vector2.zero;
        	    roam_change_time = Time.time + 3f;
        	    rb.linearVelocity = Vector2.zero;
        	    return;
        	}

			roam_direction.x = Random.Range(-10, 10);
			roam_direction.y = Random.Range(-10, 10);
			roam_change_time = Time.time + 2f;
        }

        sprite_renderer.flipX = roam_direction.x > 0;
        if(!animator.GetBool("isMoving") && rb.linearVelocity != Vector2.zero)
            animator.SetBool("isMoving", true);
        targetVelocity = roam_direction.normalized * 1.5f;
        rb.linearVelocity = targetVelocity;
    }
}
