/// <summary>
/// A tower that targets the closest enemy within range and fires projectiles, with upgrade support.
/// </summary>
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Stats")]
    public float range = 3f;
    public float fireRate = 1f;
    public int damage = 1;
    public int cost = 50;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Upgrade")]
    public int upgradeLevel = 0;
    public int maxUpgradeLevel = 3;
    public int[] upgradeCosts = { 75, 100, 150 };
    public int[] upgradeDamages = { 2, 3, 5 };

    private float lastFireTime;
    private Transform currentTarget;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Caches the sprite renderer.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Finds the nearest target and fires if the cooldown has elapsed.
    /// </summary>
    private void Update()
    {
        FindTarget();

        if (currentTarget != null && Time.time >= lastFireTime + fireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    /// <summary>
    /// Finds the closest enemy within range using a physics overlap circle.
    /// </summary>
    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);
        float closest = Mathf.Infinity;
        currentTarget = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < closest)
                {
                    closest = distance;
                    currentTarget = hit.transform;
                }
            }
        }
    }

    /// <summary>
    /// Instantiates a projectile aimed at the current target.
    /// </summary>
    private void Fire()
    {
        if (projectilePrefab == null || firePoint == null || currentTarget == null) return;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.Initialize(currentTarget, damage);
        }
    }

    /// <summary>
    /// Upgrades the tower to the next level if funds are available.
    /// </summary>
    public bool Upgrade()
    {
        if (upgradeLevel >= maxUpgradeLevel) return false;

        int cost = upgradeCosts[upgradeLevel];
        if (GameManager.Instance.SpendMoney(cost))
        {
            upgradeLevel++;
            damage = upgradeDamages[upgradeLevel - 1];
            return true;
        }
        return false;
    }

    /// <summary>
    /// Returns the cost of the next upgrade, or -1 if maxed out.
    /// </summary>
    public int GetUpgradeCost()
    {
        if (upgradeLevel >= maxUpgradeLevel) return -1;
        return upgradeCosts[upgradeLevel];
    }

    /// <summary>
    /// Draws the tower range gizmo in the editor.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
