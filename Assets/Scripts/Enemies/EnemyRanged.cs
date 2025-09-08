using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyRanged : EnemyBase
{
    public GameObject spellPrefab;
	public float spellDestroyTime = 4.9f;

    public List<Tuple<GameObject, float>> castSpells = new List<Tuple<GameObject, float>>();

    public EnemyRanged(int max_health, int damage, int healing, float move_speed) : base(max_health, damage, healing, move_speed){}

	void FilterSpells()
	{
		castSpells.ForEach(x => {
			if (Time.time - x.Item2 > spellDestroyTime)
				Destroy(x.Item1);
			x = null;
		});
		castSpells.RemoveAll(x => x == null);
	}

	public override void Attack()
	{
        rb.linearVelocity = Vector2.zero;
        if(animator.GetBool("isMoving"))
            animator.SetBool("isMoving", false);

        if(next_attack_time > Time.time ) return;
        
        animator.SetTrigger("isAttacking");
        next_attack_time = Time.time + attack_cooldown;
        
        Vector2 target = player.transform.position;
		Vector2 pos = transform.position;
		Vector2 direction = (target - pos).normalized;

		GameObject spellBullet = Instantiate(spellPrefab, pos, Quaternion.identity);
        spellBullet.GetComponent<SpellBullet>().damage = damage;
        spellBullet.GetComponent<SpellBullet>().hit_tag = "Player";
		spellBullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
		castSpells.Add(new Tuple<GameObject, float>(spellBullet, Time.time));

		Invoke("FilterSpells", 5f);
	}
}