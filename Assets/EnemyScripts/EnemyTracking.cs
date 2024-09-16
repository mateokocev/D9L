using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyTracking : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 10f;
    public float attackRadius = 7f;
    public float dashDistance = 12f;
    public float dashSpeed = 50f;
    public float dashCooldown = 1f;
    public float fieldOfViewAngle = 95f;
    public float rotationSpeed = 60f;
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    private NavMeshAgent agent;

    private bool playerInSight = false;
    private bool isLockedOn = false;
    private bool isDashing = false;
    private Vector3 dashTarget;
    private float lastDashTime = -Mathf.Infinity;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
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

            if (distanceToPlayer <= attackRadius && !isDashing && Time.time >= lastDashTime + dashCooldown)
            {
                StartCoroutine(PrepareAndDash());
            }

            if (!isDashing)
            {
                agent.SetDestination(player.position);
            }
        }

        if (isDashing)
        {
            PerformDash();
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
        if (angleBetween < fieldOfViewAngle / 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, obstacleMask | playerMask);
            Debug.DrawRay(transform.position, directionToPlayer * viewDistance, hit.collider == null ? Color.green : Color.red);
            Debug.Log(hit.collider);
            if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Obstacles"))
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);
                if (distanceToPlayer <= viewDistance)
                {
                    RaycastHit2D playerHit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, playerMask);
                    Debug.DrawRay(transform.position, directionToPlayer * viewDistance, playerHit.collider != null && playerHit.collider.CompareTag("Player") ? Color.blue : Color.yellow);

                    if (playerHit.collider != null && playerHit.collider.CompareTag("Player"))
                    {
                        playerInSight = true;
                        Debug.Log("player found!");
                    }
                    else
                    {
                        playerInSight = false;
                    }
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

    private IEnumerator PrepareAndDash()
    {
        agent.isStopped = true;

        yield return new WaitForSeconds(0.75f);

        DashTowardsPlayer();
    }

    private void DashTowardsPlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        dashTarget = transform.position + (Vector3)(directionToPlayer * dashDistance); 

        agent.isStopped = true;

        isDashing = true;
        lastDashTime = Time.time;
    }

    private void PerformDash()
    {
        transform.position = Vector3.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, dashTarget) <= 0.1f)
        {
            isDashing = false;
            agent.isStopped = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, fieldOfViewAngle / 2f) * transform.up * viewDistance));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, -fieldOfViewAngle / 2f) * transform.up * viewDistance));
    }
}
