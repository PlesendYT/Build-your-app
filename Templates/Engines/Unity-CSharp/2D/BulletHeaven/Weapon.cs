/// <summary>
/// Defines a weapon in the bullet heaven game with auto-attack, projectile firing, and upgrade levels.
/// </summary>
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public string weaponName = "Default Gun";
    public int damage = 10;
    public float fireRate = 0.3f;
    public float projectileSpeed = 12f;
    public int projectileCount = 1;
    public float spreadAngle = 0f;

    [Header("Upgrade")]
    public int upgradeLevel;
    public int maxUpgradeLevel = 5;

    [Header("References")]
    public GameObject projectilePrefab;

    private float fireTimer;

    /// <summary>
    /// Attempts to fire at the target if the cooldown has elapsed.
    /// </summary>
    public void TryAttack(Vector3 origin, Transform target)
    {
        fireTimer -= Time.deltaTime;
        if (fireTimer > 0) return;

        fireTimer = fireRate;
        FireProjectiles(origin, target);
    }

    /// <summary>
    /// Spawns projectiles aimed at the target, with spread if multiple projectiles are fired.
    /// </summary>
    private void FireProjectiles(Vector3 origin, Transform target)
    {
        Vector2 direction = (target.position - origin).normalized;
        float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        for (int i = 0; i < projectileCount; i++)
        {
            float angleOffset = (i - (projectileCount - 1) * 0.5f) * spreadAngle;
            float finalAngle = baseAngle + angleOffset;
            Quaternion rotation = Quaternion.Euler(0, 0, finalAngle);

            GameObject proj = Instantiate(projectilePrefab, origin, rotation);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = (Vector2)(rotation * Vector2.right) * projectileSpeed;
            }

            // Pass damage to the projectile
            Projectile projScript = proj.GetComponent<Projectile>();
            if (projScript != null)
                projScript.damage = damage;
        }
    }

    /// <summary>
    /// Increases the weapon's damage and fire rate on upgrade.
    /// </summary>
    public void Upgrade()
    {
        if (upgradeLevel >= maxUpgradeLevel) return;

        upgradeLevel++;
        damage = Mathf.FloorToInt(damage * 1.3f);
        fireRate = Mathf.Max(0.05f, fireRate * 0.9f);
        projectileCount++;
        Debug.Log(weaponName + " upgraded to level " + upgradeLevel);
    }
}
