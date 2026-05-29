/// <summary>
/// Moves a projectile in a set direction, damages the player on contact, and self-destructs after a lifetime or on boundary.
/// </summary>
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    public float lifetime = 5f;
    private float lifeTimer;

    /// <summary>
    /// Resets the lifetime timer when the projectile is enabled.
    /// </summary>
    private void OnEnable()
    {
        lifeTimer = lifetime;
    }

    /// <summary>
    /// Moves the projectile each frame and destroys it after its lifetime expires.
    /// </summary>
    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
            ReturnToPool();
    }

    /// <summary>
    /// Sets the movement direction, speed, and rotation angle for the projectile.
    /// </summary>
    public void SetDirection(Vector2 dir, float spd)
    {
        direction = dir.normalized;
        speed = spd;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    /// <summary>
    /// Damages the player on contact and returns the projectile to the pool.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerShip>()?.TakeDamage();
            ReturnToPool();
        }
        if (other.CompareTag("Boundary"))
        {
            ReturnToPool();
        }
    }

    /// <summary>
    /// Destroys the projectile game object.
    /// </summary>
    private void ReturnToPool()
    {
        Destroy(gameObject);
    }
}
