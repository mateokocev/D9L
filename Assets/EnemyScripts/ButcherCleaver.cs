using UnityEngine;

public class ButcherCleaver : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public float rotationSpeed = 50f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerHealth.maxHealth);
            }
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
