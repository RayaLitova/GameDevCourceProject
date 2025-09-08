using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamageable
{
    public int max_health = 10;
    public int current_health;
    public int damage;
    public float move_speed;
    public int engage_distance = 10;
    public int attack_range = 3;
    public int healing;
    public float attack_cooldown = 1f;

    protected Rigidbody2D rb;
    protected Vector2 targetVelocity;
    protected GameObject player;
    protected float next_attack_time;
    protected Animator animator;
    protected SpriteRenderer sprite_renderer;
    protected Vector2 roam_direction;
    protected float roam_change_time = 0;

    public void Start()
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
        else 
            Roam();
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

    public virtual void Attack()
    {
        rb.linearVelocity = Vector2.zero;
        if(animator.GetBool("isMoving"))
            animator.SetBool("isMoving", false);

        if(next_attack_time > Time.time ) return;
        
        animator.SetTrigger("isAttacking");
        next_attack_time = Time.time + attack_cooldown;
        PlayerStats.TakeDamage(damage, this);
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

        sprite_renderer.flipX = roam_direction.x < 0;
        if(!animator.GetBool("isMoving"))
            animator.SetBool("isMoving", true);
        targetVelocity = roam_direction.normalized * move_speed * 0.5f;
        rb.linearVelocity = targetVelocity;
    }

    public virtual void TakeDamage(int amount)
    {
        animator.SetTrigger("isHit");
        current_health -= (int)(amount);
        if (current_health < 0)
            Death();
    }

    public void Death()
    {
        PlayerStats.GainExperience(10);
        Destroy(this.gameObject);
    }
    
    public void Heal()
    {
        current_health += healing;
        if (current_health > max_health)
            current_health = max_health;
    }
}
