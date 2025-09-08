using UnityEngine;
using System;
using System.Collections.Generic;

public class EnemyRanged : EnemyBase
{
    public GameObject spellPrefab;
	public float spellDestroyTime = 4.9f;

	private ActiveSpell spell;

    public EnemyRanged(int max_health, int damage, int healing, float move_speed) : base(max_health, damage, healing, move_speed){
	}

	public void Start() {
		base.Start();
		SpellGenerator spellGenerator = GameObject.FindFirstObjectByType<SpellGenerator>();
		spell = spellGenerator.GenerateSpell(new List<string>{
			"Active",
			"IceShard",
			"A shard of ice that deals damage and slows the target.",
			"Water",
			"ModifyDamage",
			0.ToString(),
			5.ToString(),
			0.ToString()
		}) as ActiveSpell;
		Debug.Log("Enemy spell: " + spell.name + " - " + spell.description);
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
        SpellBullet sbData = spellBullet.GetComponent<SpellBullet>();
		sbData.spell = spell;
        sbData.hit_tag = "Player";
		spellBullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
		Destroy(spellBullet, spellDestroyTime);
	}
}
