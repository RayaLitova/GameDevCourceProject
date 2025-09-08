using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{
	public GameObject spellPrefab;
	public float spellDestroyTime = 4.9f;

	public List<Tuple<GameObject, float>> castSpells = new List<Tuple<GameObject, float>>();

	void Start()
	{
		PlayerStats.active_spells.Add(new ActiveSpell("Fireball", "Ball of fire", SpellType.Fire, 2, 5, 5));
	}

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			CastSpell(pos, PlayerStats.active_spells[0]);
		}
    }

	void FilterSpells()
	{
		castSpells.ForEach(x => {
			if (Time.time - x.Item2 > spellDestroyTime)
				Destroy(x.Item1);
			x = null;
		});
		castSpells.RemoveAll(x => x == null);
	}

	void CastSpell(Vector2 target, ActiveSpell spell)
	{
		Debug.Log("Casting spell at " + target);
		spell.Cast();
		Vector2 pos = transform.position;
		Vector2 direction = (target - pos).normalized;

		GameObject spellBullet = Instantiate(spellPrefab, pos, Quaternion.identity);
		spellBullet.GetComponent<SpellBullet>().spell = spell;
		spellBullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
		castSpells.Add(new Tuple<GameObject, float>(spellBullet, Time.time));

		Invoke("FilterSpells", 5f);
	}
}
