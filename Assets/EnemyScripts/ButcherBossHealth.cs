using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        // Stop all ongoing animations and set death animation
        animator.SetBool("isCharging", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("inDelay", false);
        animator.SetBool("inThrow", false);
        animator.SetBool("isDead", true);

        // Start the coroutine to delay the scene load by 5 seconds
        StartCoroutine(LoadNextSceneAfterDelay(5f));
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Load the next scene by incrementing the current scene index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public bool GetLivingState()
    {
        return livingState;
    }
}
