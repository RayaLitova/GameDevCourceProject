using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public int max_health;
    public int current_health;
    public int damage;
    public float move_speed;
    public int engage_distance = 10;
    public int attack_range = 3;
    public int healing;
    public float attack_cooldown = 1f;

    private Rigidbody2D rb;
    private Vector2 targetVelocity;
    private GameObject player;
    private float next_attack_time;
    private Animator animator;
    private SpriteRenderer sprite_renderer;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite_renderer = GetComponent<SpriteRenderer>();
        next_attack_time = Time.time;
    }

    void FixedUpdate()
    {
        var dist = Vector2.Distance(transform.position, player.transform.position);
        if (dist < attack_range)
            Attack();
        else if (dist <= engage_distance)
            MoveTowardsPlayer();
        else if (animator.GetBool("isMoving"))
            animator.SetBool("isMoving", false);
    }

    public EnemyBase(int max_health, int damage, int healing, float move_speed)
    {
        this.max_health = max_health;
        this.current_health = max_health;
        this.damage = damage;
        this.healing = healing;
        this.move_speed = move_speed;
    }

    private void MoveTowardsPlayer()
    {
        Vector2 movement = player.transform.position - transform.position;
        sprite_renderer.flipX = movement.x < 0;
        if(!animator.GetBool("isMoving"))
            animator.SetBool("isMoving", true);
        targetVelocity = movement.normalized * move_speed;
        rb.linearVelocity = targetVelocity;
    }

    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;
        if(animator.GetBool("isMoving"))
            animator.SetBool("isMoving", false);

        if(next_attack_time > Time.time ) return;
        
        animator.SetTrigger("isAttacking");
        next_attack_time = Time.time + attack_cooldown;
        PlayerStats.TakeDamage(damage, this);
    } 

    public void TakeDamage(int amount)
    {
        animator.SetTrigger("isHit");
        current_health -= (int)(amount);
        if (current_health < 0)
            Death();
    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
    
    public void Heal()
    {
        current_health += healing;
        if (current_health > max_health)
            current_health = max_health;
    }
}