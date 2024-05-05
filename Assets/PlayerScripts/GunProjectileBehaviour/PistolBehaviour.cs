using UnityEngine;

public class PistolBehaviour : MonoBehaviour
{

    public float projectileSpeed = 20.0f;
    public float projectileErrorAngle = 10.0f;
    public float projectileMaxDistance = 20.0f;

    private Vector2 projectileInitialPosition;
    private Vector3 playerMousePosition;

    void Start()
    {

        projectileInitialPosition = transform.position;
        playerMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 projectileDirection = ((Vector2)playerMousePosition - (Vector2)transform.position).normalized;
        playerMousePosition.z = 0f;

        float projectileDeviationAngle = Random.Range(-projectileErrorAngle, projectileErrorAngle);
        Quaternion projectileDeviation = Quaternion.Euler(0, 0, projectileDeviationAngle);
        projectileDirection = projectileDeviation * projectileDirection;

        GetComponent<Rigidbody2D>().velocity = projectileDirection * projectileSpeed;
    }

    void Update()
    {

        if (Vector2.Distance(projectileInitialPosition, transform.position) >= projectileMaxDistance)
        {

            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag(""))
        {
        }
    }
}