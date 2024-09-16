using UnityEngine;
using UnityEngine.AI;

public class ShotgunEnemy : MonoBehaviour
{
    public Transform player;
    public float viewDistance = 10f;
    public float shootingRange = 6f;
    public float shootingCooldown = 1f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20f;
    public int bulletsPerShot = 5;
    public float bulletSpreadAngle = 15f;
    public float rotationSpeed = 60f; 
    public LayerMask obstacleMask;
    public LayerMask playerMask;
    private NavMeshAgent agent;

    private bool playerInSight = false;
    private bool isLockedOn = false;
    private float lastShotTime = -Mathf.Infinity;

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

            if (distanceToPlayer <= shootingRange)
            {
                agent.isStopped = true;
                ShootAtPlayer();
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
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

    private void ShootAtPlayer()
    {
        if (Time.time >= lastShotTime + shootingCooldown)
        {
            lastShotTime = Time.time;
            ShootBulletSpread();
        }
    }

    private void ShootBulletSpread()
    {
        for (int i = 0; i < bulletsPerShot; i++)
        {
            float randomAngle = Random.Range(-bulletSpreadAngle / 2f, bulletSpreadAngle / 2f);
            Vector2 directionToPlayer = (player.position - bulletSpawnPoint.position).normalized;
            Vector2 bulletDirection = RotateVector2(directionToPlayer, randomAngle);
            ShootBullet(bulletDirection);
        }
    }

    private void ShootBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.transform.up = direction;
    }

    private Vector2 RotateVector2(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float tx = vector.x;
        float ty = vector.y;
        return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, 95f / 2f) * transform.up * viewDistance));
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Quaternion.Euler(0, 0, -95f / 2f) * transform.up * viewDistance));
    }
}