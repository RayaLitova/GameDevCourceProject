using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpells : MonoBehaviour
{

	public GameObject spellPrefab;

	public List<Tuple<GameObject, float>> castSpells = new List<Tuple<GameObject, float>>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
		{
			Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			CastSpell(pos);
		}
    }

	void FilterSpells()
	{
		castSpells.ForEach(x => {
			if (Time.time - x.Item2 > 4.9f)
				Destroy(x.Item1);
			x = null;
		});
		castSpells.RemoveAll(x => x == null);
	}

	void CastSpell(Vector2 target)
	{
		Debug.Log("Casting spell at " + target);

		Vector2 pos = transform.position;
		Vector2 direction = (target - pos).normalized;

		GameObject spell = Instantiate(spellPrefab, pos, Quaternion.identity);

		spell.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
		castSpells.Add(new Tuple<GameObject, float>(spell, Time.time));

		Invoke("FilterSpells", 5f);
	}
}
