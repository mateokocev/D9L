using UnityEngine;
using UnityEngine.AI;

public class PistolEnemy : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 10f;
    public float shootingRange = 8f;
    public float shootingCooldown = 0.75f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 30f;
    public float rotationSpeed = 60f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public AudioClip gunshotSound;
    private AudioSource audioSource;
    private NavMeshAgent agent;
    private Collider2D enemyCollider;
    private Animator animator;
    private PistolEnemyHealth pistolEnemyHealth;

    private bool playerInSight = false;
    private bool isLockedOn = false;
    private float lastShotTime = -Mathf.Infinity;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        pistolEnemyHealth = GetComponent<PistolEnemyHealth>();
        animator.SetBool("isWalking", false);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {

        if (!pistolEnemyHealth.GetLivingState())
        {
            isLockedOn = false;
            agent.isStopped = true;
            enemyCollider.enabled = false;
        }
        else
        {
            if (!isLockedOn)
            {
                CheckLineOfSight();
            }

            if (playerInSight || isLockedOn)
            {
                isLockedOn = true;
                RotateTowardsPlayer();

                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                if (distanceToPlayer <= shootingRange && HasClearLineOfSight())
                {
                    agent.isStopped = true;
                    animator.SetBool("isWalking", false);
                    ShootAtPlayer();
                }
                else
                {
                    agent.isStopped = false;
                    animator.SetBool("isWalking", true);
                    agent.SetDestination(player.position);
                }
            }
        }
    }

    private void RotateTowardsPlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void CheckLineOfSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetween = Vector2.Angle(transform.up, directionToPlayer);

        Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
        if (angleBetween < 95f / 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, obstacleMask | playerMask);
            Debug.DrawRay(transform.position, directionToPlayer * viewDistance, hit.collider == null ? Color.green : Color.red);

            if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
            {
                RaycastHit2D playerHit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, playerMask);
                Debug.DrawRay(transform.position, directionToPlayer * viewDistance, playerHit.collider != null && playerHit.collider.CompareTag("Player") ? Color.blue : Color.yellow);

                if (playerHit.collider != null && playerHit.collider.CompareTag("Player"))
                {
                    playerInSight = true;
                }
                else
                {
                    playerInSight = false;
                }
            }
            else
            {
                playerInSight = false;
            }
        }
        else
        {
            playerInSight = false;
        }
    }

    private bool HasClearLineOfSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, shootingRange, obstacleMask | playerMask);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;
    }

    private void ShootAtPlayer()
    {
        if (Time.time >= lastShotTime + shootingCooldown)
        {
            lastShotTime = Time.time;
            ShootBullet();
        }
    }

    private void ShootBullet()
    {
        Vector2 directionToPlayer = (player.position - bulletSpawnPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.up = directionToPlayer;
        audioSource.PlayOneShot(gunshotSound);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, 95f / 2f) * transform.up * viewDistance));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, -95f / 2f) * transform.up * viewDistance));
    }
}
