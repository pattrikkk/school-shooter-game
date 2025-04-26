using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] float _detectionRadius = 10f;  // How close the player needs to be
    [SerializeField] float _rotationSpeed = 5f;    // Speed at which the enemy rotates
    [SerializeField] Animator _animator;
    [SerializeField] private int _health = 3;
    [SerializeField] public string enemyDescription;
    [SerializeField] CapsuleCollider _capsuleCollider;

    private Transform _player;
    private NavMeshAgent _agent;
    private bool _playerInRange = false;
    private bool _wasGazedAt = false;
    private bool _isDead = false; // Added to prevent actions after death


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
        _animator.SetBool("CanShoot", true);
    }

    void Update()
    {
        if (_player == null || _isDead) return;

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

    public void TakeDamage(int damageAmount)
    {
        if (_isDead) return; // Prevent taking damage if already dead

        _health -= damageAmount;
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health: " + _health);

        if (_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;

        if (_animator != null)
        {
            _animator.SetBool("CanShoot", false);
            _animator.SetBool("IsDead", true); // Use the parameter name
        }
        //  Disable the NavMeshAgent
        if (_agent != null)
        {
            _agent.enabled = false;
        }

        //  Disable the collider so it doesn't get hit again.
        if (_capsuleCollider != null)
        {
            _capsuleCollider.enabled = false;
        }

        StartCoroutine(DisableAnimatorAfterDelay(5f));
        //Destroy(gameObject, 4f); // Adjust the time as needed for your death animation
    }

    private IEnumerator DisableAnimatorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _animator.enabled = false;
    }


    void OnDrawGizmosSelected()
    {
        // Just to visualize detection radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}
