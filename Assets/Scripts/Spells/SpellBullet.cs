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

			Vector2 position = Vector2.Lerp(transform.position, other.transform.position, 0.5f);

			float height = other.transform.position.y - 0.8f;
			Debug.Log("Height: " + height);

			GameObject effect = Instantiate(spell.effectPrefab, position, Quaternion.identity);
			effect.GetComponent<UnityEngine.VFX.VisualEffect>().SetFloat("height", height);
			Destroy(effect, 2f);

			Destroy(gameObject);
		}
	}
}
