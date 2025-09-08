using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
	public GameObject spellPrefab;
	public float spellDestroyTime = 4.9f;
	
	private SpriteRenderer renderer;

	void Start()
	{
		renderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		if(PlayerStats.color != Color.white)
			Invoke("ResetColor", 3f);
		renderer.color = PlayerStats.color;
	}

	public void ResetColor()
	{
		PlayerStats.color = Color.white;
	}

	public void CastSpell(Vector2 target, int spell_index)
	{
		if(spell_index >= PlayerStats.active_spells.Count) return; 
		ActiveSpell spell = PlayerStats.active_spells[spell_index];
		if(!spell.Cast()) return;
		Vector2 pos = transform.position;
		Vector2 direction = (target - pos).normalized;

		GameObject spellBullet = Instantiate(spellPrefab, pos, Quaternion.identity);
		spellBullet.GetComponent<SpellBullet>().spell = spell;
		spellBullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
		
		Destroy(spellBullet, spellDestroyTime);
	}
}
