/// <summary>
/// An enemy that follows waypoints toward the base, taking damage and rewarding money on death.
/// </summary>
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 2f;
    public int health = 3;
    public int reward = 25;
    public int damageToBase = 1;

    private Transform[] waypoints;
    private int currentWaypoint;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Caches the sprite renderer component.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Finds and caches the waypoint path from the scene.
    /// </summary>
    private void Start()
    {
        GameObject wpParent = GameObject.Find("Waypoints");
        if (wpParent != null)
        {
            waypoints = new Transform[wpParent.transform.childCount];
            for (int i = 0; i < wpParent.transform.childCount; i++)
            {
                waypoints[i] = wpParent.transform.GetChild(i);
            }
        }
    }

    /// <summary>
    /// Moves toward the current waypoint and progresses along the path.
    /// </summary>
    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypoint];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                ReachedEnd();
            }
        }
    }

    /// <summary>
    /// Damages the base and destroys the enemy when it reaches the end of the path.
    /// </summary>
    private void ReachedEnd()
    {
        GameManager.Instance.TakeDamage(damageToBase);
        Destroy(gameObject);
    }

    /// <summary>
    /// Reduces health and kills the enemy if health reaches zero.
    /// </summary>
    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Awards money and destroys the enemy upon death.
    /// </summary>
    private void Die()
    {
        GameManager.Instance.AddMoney(reward);
        Destroy(gameObject);
    }
}
