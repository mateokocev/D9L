using UnityEngine;

public class WolfBossHealth : MonoBehaviour
{
    public int maxHealth = 60;
    private int currentHealth;
    private Animator animator;

    private bool livingState = true;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        livingState = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("inThrow", false);
        animator.SetBool("isDead", true);
    }

    public bool GetLivingState()
    {
        return livingState;
    }
}
