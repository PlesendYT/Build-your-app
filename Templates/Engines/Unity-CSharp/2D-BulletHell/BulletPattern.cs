/// <summary>
/// Fires a circular spread pattern of bullets that can optionally rotate over time.
/// </summary>
using UnityEngine;

public class BulletPattern : MonoBehaviour
{
    [Header("Pattern Settings")]
    public GameObject bulletPrefab;
    public int bulletCount = 12;
    public float fireRate = 1.5f;
    public float bulletSpeed = 4f;
    public float spreadAngle = 360f;
    public bool rotateOverTime;
    public float rotationSpeed = 30f;
    private float fireTimer;
    private float currentAngle;

    /// <summary>
    /// Rotates the pattern if enabled, and fires bullets on a timer.
    /// </summary>
    private void Update()
    {
        if (rotateOverTime)
            currentAngle += rotationSpeed * Time.deltaTime;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Fire();
            fireTimer = fireRate;
        }
    }

    /// <summary>
    /// Instantiates a spread of bullets evenly distributed across the spread angle.
    /// </summary>
    private void Fire()
    {
        float angleStep = spreadAngle / bulletCount;
        float startAngle = currentAngle - spreadAngle / 2f;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Projectile proj = bullet.GetComponent<Projectile>();
            if (proj != null)
                proj.SetDirection(direction, bulletSpeed);
        }
    }
}
