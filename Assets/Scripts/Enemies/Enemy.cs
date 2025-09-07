using UnityEngine;

public class Enemy : EnemyBase
{
    public Enemy(int max_health, int damage, int healing, float move_speed) : base(max_health, damage, healing, move_speed) {}
}