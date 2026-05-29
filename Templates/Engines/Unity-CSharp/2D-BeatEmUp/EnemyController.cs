/// <summary>
/// Controls enemy behavior including chasing the player, attacking, and taking damage.
/// </summary>
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public int health = 2;
    public int damage = 1;
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public float chaseRange = 6f;

    [Header("Combat")]
    public float attackCooldown = 1.5f;
    private float lastAttackTime;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Caches component references and finds the player transform.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    /// <summary>
    /// Moves toward the player if within chase range, attacks if within attack range, or idles otherwise.
    /// </summary>
    private void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= chaseRange && distance > attackRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
            spriteRenderer.flipX = direction.x < 0;
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
    /// Attacks the player if the cooldown has elapsed.
    /// </summary>
    private void TryAttack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            player.GetComponent<PlayerCombat>().TakeDamage(damage);
        }
    }

    /// <summary>
    /// Reduces health by the given damage amount and destroys the enemy if health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Destroys the enemy game object.
    /// </summary>
    private void Die()
    {
        Destroy(gameObject);
    }
}
