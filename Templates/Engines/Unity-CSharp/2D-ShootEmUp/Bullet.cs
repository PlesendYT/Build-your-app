/// <summary>
/// A bullet that travels upward and damages either the player or enemies on collision.
/// </summary>
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 12f;
    public int damage = 1;
    public bool isEnemyBullet = false;
    public float lifetime = 3f;

    private Rigidbody2D rb;

    /// <summary>
    /// Caches the rigidbody and schedules auto-destruction.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Sets the bullet's upward velocity on start.
    /// </summary>
    private void Start()
    {
        rb.linearVelocity = transform.up * speed;
    }

    /// <summary>
    /// Deals damage to the hit target (player or enemy) and destroys the bullet.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemyBullet && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerShip>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!isEnemyBullet && other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boundary"))
        {
            Destroy(gameObject);
        }
    }
}
