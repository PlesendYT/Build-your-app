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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        FindTarget();

        if (currentTarget != null && Time.time >= lastFireTime + fireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

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

    public int GetUpgradeCost()
    {
        if (upgradeLevel >= maxUpgradeLevel) return -1;
        return upgradeCosts[upgradeLevel];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
