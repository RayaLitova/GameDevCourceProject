using UnityEngine;

public class SpellBullet : MonoBehaviour
{
    public ActiveSpell spell = null;
	public int damage = 0;
	public string hit_tag = "Enemy";

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag(hit_tag))
		{
			Debug.Log("Spell hit an enemy!");
			if(hit_tag == "Enemy")
            	spell.OnHit(other.GetComponent<IDamageable>());
			else
				PlayerStats.TakeDamage(damage);
				
			Destroy(gameObject);
		}
	}
}
