using UnityEngine;

public class PistolBehaviour : MonoBehaviour
{
    public float projectileSpeed = 20.0f;
    public float projectileErrorAngle = 10.0f;
    public float projectileMaxDistance = 20.0f;
    public int damage = 1;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "MeleeEnemy":
                MeleeEnemyHealth meleeHealth = collision.collider.GetComponent<MeleeEnemyHealth>();
                if (meleeHealth != null)
                {
                    meleeHealth.TakeDamage(damage);
                }
                Destroy(gameObject);
                break;

            case "PistolEnemy":
                PistolEnemyHealth pistolHealth = collision.collider.GetComponent<PistolEnemyHealth>();
                if (pistolHealth != null)
                {
                    pistolHealth.TakeDamage(damage);
                }
                Destroy(gameObject);
                break;

            case "RifleEnemy":
                RifleEnemyHealth rifleHealth = collision.collider.GetComponent<RifleEnemyHealth>();
                if (rifleHealth != null)
                {
                    rifleHealth.TakeDamage(damage);
                }
                Destroy(gameObject);
                break;

            case "ShotgunEnemy":
                ShotgunEnemyHealth shotgunHealth = collision.collider.GetComponent<ShotgunEnemyHealth>();
                if (shotgunHealth != null)
                {
                    shotgunHealth.TakeDamage(damage);
                }
                Destroy(gameObject);
                break;

            case "SniperEnemy":
                SniperEnemyHealth sniperHealth = collision.collider.GetComponent<SniperEnemyHealth>();
                if (sniperHealth != null)
                {
                    sniperHealth.TakeDamage(damage);
                }
                Destroy(gameObject);
                break;

            case "Wall":
                Destroy(gameObject);
                break;

            default:
                break;
        }
    }
}
