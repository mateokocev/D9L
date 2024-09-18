using UnityEngine;

public class SniperEnemyHealth : MonoBehaviour
{
    public int maxHealth = 1;
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
        Debug.Log("Sniper enemy took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        livingState = false;

        animator.SetBool("isDead", true);
    }

    public bool GetLivingState()
    {
        return livingState;
    }
}
