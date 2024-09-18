using UnityEngine;

public class carScript : MonoBehaviour
{
    // Speed at which the GameObject moves down (modifiable in the Inspector)
    public float speed = 5f;
    public int damageAmount = 10;  // The amount of damage to deal to the player (modifiable in the Inspector)

    // Update is called once per frame
    void Update()
    {
        // Move the GameObject down continuously
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }

    // Detect collision with the player
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object that collided with the car has the tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PlayerHealth component from the player GameObject
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // If the PlayerHealth component exists, invoke TakeDamage
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
