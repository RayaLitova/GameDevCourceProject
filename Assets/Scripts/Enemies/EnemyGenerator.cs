using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject ranged_enemy;
    public GameObject melee_enemy;
    public int ranged_chance = 50;

    void Start()
    {
        Invoke("SpawnEnemy", 1f);
    }

    void SpawnEnemy()
    {
        if(Random.Range(0, 100) > ranged_chance)
            Instantiate(ranged_enemy, transform.position, Quaternion.identity);
        else
            Instantiate(melee_enemy, transform.position, Quaternion.identity);
        
        Invoke("SpawnEnemy", Random.Range(2, 10));
    }
}
