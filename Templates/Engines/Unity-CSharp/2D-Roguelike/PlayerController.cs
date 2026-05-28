using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public int maxHealth = 10;
    public int attackDamage = 2;
    public int currentHealth;
    public int level = 1;

    [Header("Movement")]
    public float moveDelay = 0.2f;
    private float lastMoveTime;
    private bool isDead;

    private DungeonGenerator dungeon;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dungeon = FindAnyObjectByType<DungeonGenerator>();
        currentHealth = maxHealth;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (isDead || !context.performed || Time.time < lastMoveTime + moveDelay) return;

        Vector2 input = context.ReadValue<Vector2>();
        Vector2Int direction = new Vector2Int(
            Mathf.RoundToInt(input.x),
            Mathf.RoundToInt(input.y)
        );

        if (direction == Vector2Int.zero) return;

        Vector3 targetPos = transform.position + new Vector3(direction.x, direction.y, 0);
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y));

        if (dungeon.GetTile(gridPos.x, gridPos.y) != DungeonGenerator.TileType.Wall)
        {
            Collider2D hit = Physics2D.OverlapPoint(targetPos);
            if (hit != null && hit.TryGetComponent<EnemyController>(out var enemy))
            {
                Attack(enemy);
            }
            else
            {
                transform.position = targetPos;
                lastMoveTime = Time.time;
            }
        }
    }

    private void Attack(EnemyController enemy)
    {
        enemy.TakeDamage(attackDamage);
        lastMoveTime = Time.time;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        spriteRenderer.color = Color.red;
        GameManager.Instance.GameOver();
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}

public class EnemyController : MonoBehaviour
{
    public int health = 3;
    public int damage = 1;
    public int xpReward = 10;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
