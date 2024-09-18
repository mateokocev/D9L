using UnityEngine;
using UnityEngine.AI;

public class SniperEnemy : MonoBehaviour
{
    public Transform player;
    public GameObject sniperBulletPrefab;
    public Transform bulletSpawnPointSniper;
    public float shootingCooldown = 3f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public float viewDistance = 40f;
    public float fieldOfViewAngle = 95f;
    public float rotationSpeed = 120f;
    public LineRenderer lineRenderer;
    public AudioClip gunshotSound;
    private AudioSource audioSource;
    private Collider2D enemyCollider;
    private Animator animator;
    private SniperEnemyHealth sniperEnemyHealth;

    private bool isLockedOn = false;
    private float lastShotTime = -Mathf.Infinity;

    private void Start()
    {
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        sniperEnemyHealth = GetComponent<SniperEnemyHealth>();
        animator.SetBool("isDead", false);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!sniperEnemyHealth.GetLivingState())
        {
            isLockedOn = false;
            enemyCollider.enabled = false;
        }
        else
        {
            if (!isLockedOn)
            {
                CheckLineOfSight();
            }

            if (isLockedOn)
            {
                RotateTowardsPlayer();

                DrawLaserLine();

                if (Time.time >= lastShotTime + shootingCooldown && HasClearLineOfSight())
                {
                    Shoot();
                }
            }
            else
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                }
            }
        }
    }

    private void CheckLineOfSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetween = Vector2.Angle(transform.up, directionToPlayer);

        if (angleBetween < fieldOfViewAngle / 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, obstacleMask | playerMask);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                isLockedOn = true;
            }
        }
    }

    private bool HasClearLineOfSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, obstacleMask | playerMask);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    private void RotateTowardsPlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void DrawLaserLine()
    {
        Vector2 directionToPlayer = (player.position - bulletSpawnPointSniper.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(bulletSpawnPointSniper.position, directionToPlayer, viewDistance, obstacleMask | playerMask);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
        }

        lineRenderer.positionCount = 2;

        lineRenderer.SetPosition(0, bulletSpawnPointSniper.position);

        if (hit.collider != null)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, bulletSpawnPointSniper.position + (Vector3)(directionToPlayer * viewDistance));
        }
    }


    private void Shoot()
    {
        lastShotTime = Time.time;
        Instantiate(sniperBulletPrefab, bulletSpawnPointSniper.position, bulletSpawnPointSniper.rotation);
        audioSource.PlayOneShot(gunshotSound);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, fieldOfViewAngle / 2f) * transform.up * viewDistance));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, -fieldOfViewAngle / 2f) * transform.up * viewDistance));
    }
}