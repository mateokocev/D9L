using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class RifleEnemy : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 12f;
    public float shootingRange = 10f;
    public float burstCooldown = 0.75f;
    public GameObject bulletPrefab;
    public Transform rifleBulletSpawnPoint;
    public float bulletSpeed = 35f;
    public float timeBetweenShots = 0.1f;
    public int bulletsPerBurst = 3;
    public float rotationSpeed = 5f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    private NavMeshAgent agent;
    private Collider2D enemyCollider;
    private Animator animator;
    private RifleEnemyHealth rifleEnemyHealth;

    private bool playerInSight = false;
    private bool isLockedOn = false;
    private float lastBurstTime = -Mathf.Infinity;
    private bool isShootingBurst = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        rifleEnemyHealth = GetComponent<RifleEnemyHealth>();
        animator.SetBool("isWalking", false);
    }

    private void Update()
    {
        if (!rifleEnemyHealth.GetLivingState())
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

                    if (Time.time >= lastBurstTime + burstCooldown && !isShootingBurst)
                    {
                        StartCoroutine(ShootBurst());
                    }
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
                    Debug.Log("Player found!");
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

    private IEnumerator ShootBurst()
    {
        isShootingBurst = true;
        lastBurstTime = Time.time;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            ShootBullet();
            yield return new WaitForSeconds(timeBetweenShots);
        }

        isShootingBurst = false;
    }

    private void ShootBullet()
    {
        Vector2 directionToPlayer = (player.position - rifleBulletSpawnPoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, rifleBulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.up = directionToPlayer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, 95f / 2f) * transform.up * viewDistance));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, -95f / 2f) * transform.up * viewDistance));
    }
}
