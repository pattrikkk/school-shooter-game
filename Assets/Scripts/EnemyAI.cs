using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float _detectionRadius = 10f;  // How close the player needs to be
    [SerializeField] float _rotationSpeed = 5f;    // Speed at which the enemy rotates

    private Transform _player;
    private NavMeshAgent _agent;
    private bool _playerInRange = false;
    private bool _wasGazedAt = false;
    private float _currentGazeTime = 0f;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        CheckGaze.OnGazeAction += IsBeingGazedOn;
    }

    private void OnDestroy()
    {
        CheckGaze.OnGazeAction -= IsBeingGazedOn;
    }

    public void Setup(Transform player)
    {
        _player = player;
    }

    private void IsBeingGazedOn()
    {
        _wasGazedAt = true;
    }

    void Update()
    {
        if (_player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= _detectionRadius && _wasGazedAt)
        {
            _playerInRange = true;
            Vector3 directionToPlayer = _player.position - transform.position;
            directionToPlayer.y = 0; // Keep the rotation horizontal

            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        }
        else
        {
            _playerInRange = false;
        }


        //_agent.SetDestination(_player.position);  // Follow Player.  Remove/Comment this out if you don't want it to follow.
        //_agent.ResetPath(); //stop
    }

    void OnDrawGizmosSelected()
    {
        // Just to visualize detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
