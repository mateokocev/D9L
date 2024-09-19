using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 2;
    private int currentHealth;
  
    void Start()
    {

        currentHealth = maxHealth;
        
        
    }
    private void Update()
    {
        
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
   


}