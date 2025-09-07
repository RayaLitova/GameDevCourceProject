using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Training Dummy took " + amount + " damage.");
        animator.SetBool("IsHit", true);
        Invoke("ResetHit", 2f);
    }

    private void ResetHit()
    {
        animator.SetBool("IsHit", false);
    }
}
