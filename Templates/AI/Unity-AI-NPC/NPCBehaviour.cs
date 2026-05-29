using UnityEngine;
using UnityEngine.AI;

/// <summary>NPC AI behaviour with a simple state machine: patrol, chase, and idle.</summary>
public class NPCBehaviour : MonoBehaviour
{
    // NPC states for the finite state machine
    private enum NpcState { Idle, Patrol, Chase }

    [Header("Movement")]
    [SerializeField] private float _patrolRadius = 10f;
    [SerializeField] private float _chaseSpeed = 5f;
    [SerializeField] private float _walkSpeed = 2f;

    [Header("Detection")]
    [SerializeField] private float _detectionRange = 8f;
    [SerializeField] private LayerMask _playerLayer;

    private NavMeshAgent _agent;
    private NpcState _currentState = NpcState.Idle;
    private Transform _player;
    private Vector3 _homePosition;
    private float _idleTimer;

    /// <summary>Cache components and initialize state.</summary>
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _homePosition = transform.position;
        _agent.speed = _walkSpeed;
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        SwitchState(NpcState.Patrol);
    }

    /// <summary>Main update loop: run the current state's logic.</summary>
    private void Update()
    {
        switch (_currentState)
        {
            case NpcState.Idle:
                UpdateIdle();
                break;
            case NpcState.Patrol:
                UpdatePatrol();
                break;
            case NpcState.Chase:
                UpdateChase();
                break;
        }
    }

    /// <summary>Transition to a new state and run its entry logic.</summary>
    private void SwitchState(NpcState newState)
    {
        _currentState = newState;

        switch (newState)
        {
            case NpcState.Idle:
                _idleTimer = 3f;
                _agent.isStopped = true;
                break;
            case NpcState.Patrol:
                _agent.speed = _walkSpeed;
                _agent.isStopped = false;
                SetRandomPatrolDestination();
                break;
            case NpcState.Chase:
                _agent.speed = _chaseSpeed;
                _agent.isStopped = false;
                break;
        }
    }

    /// <summary>Idle: wait for a few seconds then return to patrol.</summary>
    private void UpdateIdle()
    {
        _idleTimer -= Time.deltaTime;
        if (_idleTimer <= 0f)
            SwitchState(NpcState.Patrol);

        // Check if player enters detection range while idle
        if (PlayerInRange())
            SwitchState(NpcState.Chase);
    }

    /// <summary>Patrol: walk to random waypoints within the patrol radius.</summary>
    private void UpdatePatrol()
    {
        // If the agent reached its destination, idle briefly
        if (!_agent.pathPending && _agent.remainingDistance < 1f)
            SwitchState(NpcState.Idle);

        // Chase the player if they enter detection range
        if (PlayerInRange())
            SwitchState(NpcState.Chase);
    }

    /// <summary>Chase: pursue the player until they escape detection range.</summary>
    private void UpdateChase()
    {
        if (_player == null)
        {
            SwitchState(NpcState.Patrol);
            return;
        }

        _agent.SetDestination(_player.position);

        // Stop chasing if the player moves out of range
        if (!PlayerInRange())
            SwitchState(NpcState.Patrol);
    }

    /// <summary>Check if the player is within detection range.</summary>
    private bool PlayerInRange()
    {
        if (_player == null) return false;
        float distance = Vector3.Distance(transform.position, _player.position);
        return distance <= _detectionRange;
    }

    /// <summary>Set a random destination within the patrol radius.</summary>
    private void SetRandomPatrolDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _patrolRadius;
        randomDirection += _homePosition;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _patrolRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
        }
    }

    /// <summary>Visualize the detection radius in the editor.</summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
