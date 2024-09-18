using UnityEngine;

public class RifleEnemyHealth : MonoBehaviour
{
    public int maxHealth = 2;
    private int currentHealth;
    private Animator animator;

    private bool livingState = true;

    public GameObject rifleWeaponChangePrefab;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Rifle enemy took " + damage + " damage. Current health: " + currentHealth);

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

        if (Random.Range(0f, 1f) <= 0.1f)
        {
            DropRifleWeaponChange();
        }
    }

    void DropRifleWeaponChange()
    {
        Instantiate(rifleWeaponChangePrefab, transform.position, Quaternion.identity);
    }

    public bool GetLivingState()
    {
        return livingState;
    }
}
