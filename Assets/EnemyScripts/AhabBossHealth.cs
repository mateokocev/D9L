using UnityEngine;

public class AhabBossHealth : MonoBehaviour
{
    public int maxHealth = 100;
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
        Debug.Log("Ahab boss took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        livingState = false;

        animator.SetBool("isWalking", false);
        animator.SetBool("isDead", true);
    }

    public bool GetLivingState()
    {
        return livingState;
    }
}
