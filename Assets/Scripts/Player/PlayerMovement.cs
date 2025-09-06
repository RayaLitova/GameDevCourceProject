using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float smoothTime = 0.1f;
    
    private Vector2 movement;
    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private Vector2 targetVelocity;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        movement = new Vector2(horizontal, vertical);
        
        if (movement.magnitude > 1)
            movement = movement.normalized;
    }
    
    void MovePlayer()
    {
        if (rb == null) return;
        
        targetVelocity = movement * moveSpeed;
        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, targetVelocity, ref currentVelocity, smoothTime);
    }
}
