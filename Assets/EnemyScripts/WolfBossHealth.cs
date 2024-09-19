using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        // Start a coroutine to delay the scene load by 5 seconds
        StartCoroutine(LoadNextSceneAfterDelay(5f));
    }

    IEnumerator LoadNextSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Load the next scene by incrementing the current scene index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public bool GetLivingState()
    {
        return livingState;
    }
}
