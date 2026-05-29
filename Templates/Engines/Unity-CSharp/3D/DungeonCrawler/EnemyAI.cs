/// <summary>
/// Dungeon enemy with patrol, chase, and attack states for dungeon crawler combat.
/// </summary>
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack }
    public AIState currentState = AIState.Patrol;

    [Header("Patrol")]
    public float patrolRadius = 5f, patrolSpeed = 2f, waitTime = 2f;
    private Vector3 patrolCenter, targetPos;
    private float waitTimer;

    [Header("Chase")]
    public float detectionRange = 10f, chaseSpeed = 4f, loseInterestRange = 15f;
    private Transform player;

    [Header("Attack")]
    public int damage = 15;
    public float attackRange = 1.5f, attackCooldown = 1.5f;
    private float nextAttackTime;

    [Header("Health")]
    public int maxHealth = 50;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        patrolCenter = transform.position;
        SetNewPatrolTarget();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;
        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= attackRange && Time.time >= nextAttackTime) currentState = AIState.Attack;
        else if (dist <= detectionRange) currentState = AIState.Chase;
        else if (currentState == AIState.Chase && dist > loseInterestRange) { currentState = AIState.Patrol; SetNewPatrolTarget(); }

        switch (currentState)
        {
            case AIState.Patrol: MoveTowards(targetPos, patrolSpeed); if (Vector3.Distance(transform.position, targetPos) < 0.5f) { waitTimer += Time.deltaTime; if (waitTimer >= waitTime) { SetNewPatrolTarget(); waitTimer = 0f; } } break;
            case AIState.Chase: MoveTowards(player.position, chaseSpeed); break;
            case AIState.Attack:
                transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
                player.GetComponent<PlayerController>()?.TakeDamage(damage);
                nextAttackTime = Time.time + attackCooldown;
                currentState = AIState.Chase;
                break;
        }
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 dir = (target - transform.position).normalized; dir.y = 0f;
        transform.position += dir * speed * Time.deltaTime;
        if (dir.magnitude > 0.1f) transform.rotation = Quaternion.LookRotation(dir);
    }

    private void SetNewPatrolTarget()
    {
        Vector2 r = Random.insideUnitCircle * patrolRadius;
        targetPos = patrolCenter + new Vector3(r.x, 0f, r.y);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) { GameManager.Instance.EnemyDefeated(Random.Range(5, 20)); Destroy(gameObject); }
        else currentState = AIState.Chase;
    }
}
