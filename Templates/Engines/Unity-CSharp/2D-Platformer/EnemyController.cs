/// <summary>
/// Controls enemy patrol between two points, damages the player on contact, and has basic health.
/// </summary>
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;
    public Transform patrolPointA;
    public Transform patrolPointB;
    private Vector2 targetPoint;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [Header("Health")]
    public int health = 1;

    /// <summary>
    /// Caches component references and sets the initial patrol target.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPoint = patrolPointB.position;
    }

    /// <summary>
    /// Moves the enemy between patrol points each physics frame.
    /// </summary>
    private void FixedUpdate()
    {
        Patrol();
    }

    /// <summary>
    /// Moves toward the current patrol target and switches direction when close.
    /// </summary>
    private void Patrol()
    {
        float distanceToTarget = Vector2.Distance(transform.position, targetPoint);
        if (distanceToTarget < 0.5f)
        {
            targetPoint = targetPoint == (Vector2)patrolPointB.position
                ? patrolPointA.position
                : patrolPointB.position;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        Vector2 direction = ((Vector2)targetPoint - rb.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    /// <summary>
    /// Damages the player on contact.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    /// <summary>
    /// Reduces health and destroys the enemy when health reaches zero.
    /// </summary>
    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
