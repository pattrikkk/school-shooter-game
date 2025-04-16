using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform player;             // Reference to the player
    [SerializeField] float detectionRadius = 10f;  // How close the player needs to be

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            // Player is close enough, follow him
            agent.SetDestination(player.position);
        }
        else
        {
            // Stop moving when player is out of range
            agent.ResetPath();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Just to visualize detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
