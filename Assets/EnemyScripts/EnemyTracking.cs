using UnityEngine;
using UnityEngine.AI;

public class EnemyTracking : MonoBehaviour
{

    [SerializeField] Transform targetPlayer;

    NavMeshAgent agent;

    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {

        agent.SetDestination(targetPlayer.position);
    }
}
