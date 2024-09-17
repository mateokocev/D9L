using UnityEngine;

public class RifleBullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifetime = 2f;
    public int damage = 1;

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
            Destroy(gameObject);
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}