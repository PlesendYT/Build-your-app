/// <summary>
/// Simple enemy AI with NavMesh chasing, attack, and death handling.
/// </summary>
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public int health = 50;
    public int damage = 10;
    public float attackRange = 2f;
    public float detectionRange = 15f;

    [Header("AI")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 5f;
    public Transform[] patrolPoints;

    private NavMeshAgent agent;
    private Transform player;
    private int currentPatrolIndex;
    private float lastAttackTime;
    public float attackCooldown = 1f;

    private enum AIState { Patrol, Chase, Attack }
    private AIState currentState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        currentState = AIState.Patrol;
    }

    private void Start()
    {
        if (patrolPoints.Length > 0)
            agent.destination = patrolPoints[0].position;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                if (distance <= detectionRange) currentState = AIState.Chase;
                break;
            case AIState.Chase:
                Chase();
                if (distance > detectionRange) currentState = AIState.Patrol;
                if (distance <= attackRange) currentState = AIState.Attack;
                break;
            case AIState.Attack:
                Attack();
                if (distance > attackRange) currentState = AIState.Chase;
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0) return;
        agent.speed = patrolSpeed;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            agent.destination = patrolPoints[currentPatrolIndex].position;
        }
    }

    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.destination = player.position;
    }

    private void Attack()
    {
        agent.isStopped = true;
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            player.GetComponent<PlayerController>().TakeDamage(damage);
        }
        agent.isStopped = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        currentState = AIState.Chase;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
