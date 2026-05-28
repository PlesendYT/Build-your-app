using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    public float lifetime = 5f;
    private float lifeTimer;

    private void OnEnable()
    {
        lifeTimer = lifetime;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
            ReturnToPool();
    }

    public void SetDirection(Vector2 dir, float spd)
    {
        direction = dir.normalized;
        speed = spd;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

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

    private void ReturnToPool()
    {
        Destroy(gameObject);
    }
}
