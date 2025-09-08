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
			if(hit_tag == "Enemy")
            	spell.OnHit(other.GetComponent<IDamageable>());
			else
				PlayerStats.TakeDamage(damage);

			Debug.Log("SpellBullet hit " + other.name);

			Vector2 position = Vector2.Lerp(transform.position, other.transform.position, 0.5f);

			float height = other.transform.position.y - 0.6f;

			Destroy(gameObject);

			if(spell != null) {
				GameObject effect = Instantiate(spell.effectPrefab, position, Quaternion.identity);
				UnityEngine.VFX.VisualEffect vfx = effect.GetComponent<UnityEngine.VFX.VisualEffect>();
				vfx.SetFloat("height", height);
				var gradient = vfx.GetGradient("ColorGradient");
				Debug.Log(gradient.colorKeys);

				Destroy(effect, 2f);
			} else {
				Debug.Log("No spell assigned to SpellBullet.");
			}

		}
	}
}
