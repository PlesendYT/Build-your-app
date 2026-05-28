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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

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

    private void ReachedEnd()
    {
        GameManager.Instance.TakeDamage(damageToBase);
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameManager.Instance.AddMoney(reward);
        Destroy(gameObject);
    }
}
