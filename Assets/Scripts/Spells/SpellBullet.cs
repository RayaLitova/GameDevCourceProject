using UnityEngine;

public class SpellBullet : MonoBehaviour
{
    public ActiveSpell spell;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			Debug.Log("Spell hit an enemy!");
            spell.OnHit(other.GetComponent<IDamageable>());
			Destroy(gameObject);
		}
	}
}
