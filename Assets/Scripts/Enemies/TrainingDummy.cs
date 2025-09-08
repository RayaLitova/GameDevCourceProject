using UnityEngine;

public class TrainingDummy : MonoBehaviour, IDamageable
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        animator.SetTrigger("IsHit");
    }

}
