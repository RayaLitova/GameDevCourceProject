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
        Debug.Log("Training Dummy took " + amount + " damage.");
        animator.SetTrigger("IsHit");
    }

}
