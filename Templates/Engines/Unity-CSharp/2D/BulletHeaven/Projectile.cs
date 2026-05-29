/// <summary>
/// A projectile fired by the player's weapon that travels and damages enemies on contact.
/// </summary>
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Stats")]
    public int damage = 10;
    public float lifetime = 3f;
    private float lifeTimer;

    /// <summary>
    /// Initializes the lifetime timer.
    /// </summary>
    private void OnEnable()
    {
        lifeTimer = lifetime;
    }

    /// <summary>
    /// Counts down lifetime and destroys the projectile when time runs out.
    /// </summary>
    private void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
            Destroy(gameObject);
    }

    /// <summary>
    /// On collision with an enemy, applies damage and destroys the projectile.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
