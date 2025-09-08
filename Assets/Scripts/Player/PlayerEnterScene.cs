using UnityEngine;

public class PlayerEnterScene : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f;
    
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private Vector2 targetVelocity;
    private Vector2 endPos = new Vector2(1, 0);
    private Animator animator;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void FixedUpdate()
    {
        MovePlayer();
        if(transform.position.x >= endPos.x)
        {
            animator.SetBool("Reached", true);
            Destroy(this);
            Destroy(rb);
        }
    }
    
    void MovePlayer()
    {
        if (rb == null) return;
        var speed = moveSpeed;
        targetVelocity = endPos * speed;
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, smoothTime);
    }
}
