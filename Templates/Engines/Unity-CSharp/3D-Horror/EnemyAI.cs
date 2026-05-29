/// <summary>
/// Horror enemy with patrol, chase, attack, and unpredictable behaviour patterns.
/// </summary>
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public float walkSpeed = 2f;
    public float chaseSpeed = 6f;
    public float detectionRange = 10f;
    public float chaseRange = 20f;
    public float attackRange = 2f;

    private NavMeshAgent agent;
    private Transform player;
    private Vector3 spawnPosition;
    private float lastAttackTime;
    public float attackCooldown = 2f;

    private enum AIState { Patrol, Investigate, Chase, Attack }
    private AIState currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        spawnPosition = transform.position;
        currentState = AIState.Patrol;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case AIState.Patrol:
                agent.speed = walkSpeed;
                if (distance <= detectionRange)
                {
                    currentState = AIState.Chase;
                }
                break;

            case AIState.Chase:
                agent.speed = chaseSpeed;
                agent.destination = player.position;
                if (distance > chaseRange)
                {
                    currentState = AIState.Patrol;
                    agent.destination = spawnPosition;
                }
                if (distance <= attackRange)
                {
                    currentState = AIState.Attack;
                }
                break;

            case AIState.Attack:
                agent.isStopped = true;
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;
                    player.GetComponent<PlayerController>().TakeDamage(25);
                }
                if (distance > attackRange)
                {
                    agent.isStopped = false;
                    currentState = AIState.Chase;
                }
                break;
        }
    }

    public void InvestigatePosition(Vector3 position)
    {
        currentState = AIState.Investigate;
        agent.destination = position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
