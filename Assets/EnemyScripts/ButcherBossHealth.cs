using UnityEngine;

public class ButcherBossHealth : MonoBehaviour
{
    public int maxHealth = 150;
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
        Debug.Log("Butcher boss took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        livingState = false;

        animator.SetBool("isCharging", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("inDelay", false);
        animator.SetBool("inThrow", false);
        animator.SetBool("isDead", true);
    }

    public bool GetLivingState()
    {
        return livingState;
    }
}
