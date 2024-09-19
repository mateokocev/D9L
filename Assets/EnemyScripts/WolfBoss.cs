using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WolfBoss : MonoBehaviour
{
    public Transform player;
    public GameObject shurikenPrefab;
    public Transform shurikenSpawnPoint;
    public float delayBetweenShots = 0.5f;
    public int shurikensPerAttack = 3;
    public float rotationSpeed = 60f;
    public float viewDistance = 20f;
    public float attackRange = 8f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    public AudioClip shurikenThrowSound;

    private AudioSource audioSource;
    private Animator animator;
    private WolfBossHealth wolfBossHealth;
    private NavMeshAgent agent;
    private Collider2D enemyCollider;

    private bool isLockedOn = false;
    private bool isDashing = false;
    private bool isAttacking = false;
    private bool playerInSight = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        wolfBossHealth = GetComponent<WolfBossHealth>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (!wolfBossHealth.GetLivingState())
        {
            isLockedOn = false;
            agent.isStopped = true;
            enemyCollider.enabled = false;
            animator.SetBool("isWalking", false);
            animator.SetBool("inThrow", false);
            animator.SetBool("isDead", true);
        }
        else
        {
            if (!isLockedOn)
            {
                CheckLineOfSight();
            }

            if (playerInSight || isLockedOn)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                RotateTowardsPlayer();

                if (distanceToPlayer <= attackRange && HasClearLineOfSight())
                {
                    if (isLockedOn && !isDashing && !isAttacking)
                    {
                        agent.isStopped = true;
                        animator.SetBool("isWalking", false);
                        RotateTowardsPlayer();
                        StartCoroutine(AttackPattern());
                    }
                }
                else if (distanceToPlayer >= attackRange && !isDashing && !isAttacking)
                {
                    agent.isStopped = false;
                    animator.SetBool("isWalking", true);
                    agent.SetDestination(player.position);
                }
            }

            UpdateAnimatorStates();
        }
    }

    private IEnumerator AttackPattern()
    {
        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetBool("inThrow", true);
        
        for (int i = 0; i < shurikensPerAttack; i++)
        {
            if (wolfBossHealth.GetLivingState())
            {
                RotateTowardsPlayer();
                yield return new WaitForSeconds(delayBetweenShots);
                ThrowShuriken();
            }
        }
        RotateTowardsPlayer();
        isAttacking = false;
        animator.SetBool("inThrow", false);
    }

    private void ThrowShuriken()
    {
        Vector2 directionToPlayer = (player.position - shurikenSpawnPoint.position).normalized;
        GameObject shuriken = Instantiate(shurikenPrefab, shurikenSpawnPoint.position, Quaternion.identity);
        shuriken.transform.up = directionToPlayer;

        audioSource.PlayOneShot(shurikenThrowSound);
    }

    private void CheckLineOfSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float angleBetween = Vector2.Angle(transform.up, directionToPlayer);

        if (angleBetween < 95f / 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, obstacleMask | playerMask);
            if (hit.collider != null && hit.collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
            {
                RaycastHit2D playerHit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, playerMask);

                if (playerHit.collider != null && playerHit.collider.CompareTag("Player"))
                {
                    playerInSight = true;
                    isLockedOn = HasClearLineOfSight();
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

    private void UpdateAnimatorStates()
    {
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerHealth.maxHealth);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, 95f / 2f) * transform.up * viewDistance));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, -95f / 2f) * transform.up * viewDistance));
    }
}
