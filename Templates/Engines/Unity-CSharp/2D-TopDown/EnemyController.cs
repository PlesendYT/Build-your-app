/// <summary>
/// Controls a top-down enemy that chases the player and attacks when in range.
/// </summary>
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Chase")]
    public float chaseSpeed = 3f;
    public float chaseRange = 5f;
    public float attackRange = 1f;
    public int attackDamage = 1;
    public float attackCooldown = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private float lastAttackTime;

    /// <summary>
    /// Caches references and finds the player transform.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    /// <summary>
    /// Evaluates distance to player and decides whether to chase or attack each physics frame.
    /// </summary>
    private void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRange && distance > attackRange)
        {
            ChasePlayer();
        }
        else if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            TryAttack();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Moves the enemy toward the player.
    /// </summary>
    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;
    }

    /// <summary>
    /// Attacks the player if the cooldown has elapsed.
    /// </summary>
    private void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            player.GetComponent<PlayerController>().TakeDamage(attackDamage);
        }
    }

    /// <summary>
    /// Draws chase and attack range gizmos in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
