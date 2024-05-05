using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    public Transform playerReference;
    public float detectionRange = 15f;
    public float fovDetectionAngle = 90f;
    public LayerMask obstacleMask;
    
    private EnemyState currentState = EnemyState.Idle;
    private Transform enemyReference;

    void Start()
    {
        
        enemyReference = transform;
    }

    void Update()
    {
        
        switch(currentState)
        {

            case EnemyState.Idle:
                break;

            case EnemyState.Inspect:
                break;

            case EnemyState.Attack:
                break;

            case EnemyState.Dead:
                break;
        }
    }

    void IdleBehaviour()
    {

        if (PlayerDetected())
        {

            currentState = EnemyState.Attack;
        }
    }

    void AttackBehaviour()
    {}

    void DeadBehaviour()
    {}

    bool PlayerDetected()
    {

        if (Vector2.Distance(enemyReference.position, playerReference.position) <= detectionRange)
        {

            Vector2 directionToPlayer = playerReference.position - enemyReference.position;
            float angleToPlayer = Vector2.Angle(enemyReference.forward, directionToPlayer.normalized);

            if (angleToPlayer <= fovDetectionAngle * 0.5f)
            {
                
                RaycastHit2D hit = Physics2D.Raycast(enemyReference.position, directionToPlayer.normalized, detectionRange, ~obstacleMask);

                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {

                    return true;
                }
            }
        }

        return false;
    }
}

