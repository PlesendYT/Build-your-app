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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPoint = patrolPointB.position;
    }

    private void FixedUpdate()
    {
        Patrol();
    }

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
