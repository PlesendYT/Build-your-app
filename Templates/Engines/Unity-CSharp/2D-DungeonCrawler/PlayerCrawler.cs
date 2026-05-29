/// <summary>
/// Controls the dungeon crawler player: grid-based movement, combat with enemies, and health management.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrawler : MonoBehaviour
{
    [Header("Combat")]
    public int health = 20;
    public int maxHealth = 20;
    public int attack = 5;
    public int defense = 2;
    private bool isMoving;

    private DungeonGenerator dungeonGen;

    /// <summary>
    /// Caches the dungeon generator reference.
    /// </summary>
    private void Awake()
    {
        dungeonGen = FindObjectOfType<DungeonGenerator>();
    }

    /// <summary>
    /// Handles grid-based movement input: moves to adjacent floor tiles, attacks enemies, or opens treasure.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed || isMoving) return;
        Vector2 input = context.ReadValue<Vector2>();

        Vector2Int dir = Vector2Int.zero;
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            dir = new Vector2Int((int)Mathf.Sign(input.x), 0);
        else if (Mathf.Abs(input.y) > 0)
            dir = new Vector2Int(0, (int)Mathf.Sign(input.y));

        if (dir == Vector2Int.zero) return;

        Vector3 targetPos = transform.position + new Vector3(dir.x, dir.y, 0f);
        Vector2Int targetGrid = new Vector2Int(Mathf.RoundToInt(targetPos.x), Mathf.RoundToInt(targetPos.y));

        // Check if the target tile is a valid floor tile in the dungeon
        if (dungeonGen != null && targetGrid.x >= 0 && targetGrid.x < dungeonGen.width &&
            targetGrid.y >= 0 && targetGrid.y < dungeonGen.height &&
            dungeonGen.dungeon[targetGrid.x, targetGrid.y] == DungeonGenerator.TileType.Floor)
        {
            // Check for enemies or treasure at the target position
            Collider2D[] hits = Physics2D.OverlapPointAll(targetPos);
            foreach (Collider2D hit in hits)
            {
                EnemyCrawler enemy = hit.GetComponent<EnemyCrawler>();
                if (enemy != null)
                {
                    AttackEnemy(enemy);
                    return;
                }
                TreasureChest treasure = hit.GetComponent<TreasureChest>();
                if (treasure != null)
                {
                    treasure.Open();
                    return;
                }
            }

            transform.position = targetPos;
        }
    }

    /// <summary>
    /// Attacks an enemy dealing damage based on attack minus enemy defense.
    /// </summary>
    private void AttackEnemy(EnemyCrawler enemy)
    {
        int damage = Mathf.Max(1, attack - enemy.defense);
        enemy.TakeDamage(damage);
    }

    /// <summary>
    /// Applies damage mitigated by defense and triggers game over if health reaches zero.
    /// </summary>
    public void TakeDamage(int damage)
    {
        int actual = Mathf.Max(1, damage - defense);
        health -= actual;
        if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }

    /// <summary>
    /// Heals the player up to maximum health.
    /// </summary>
    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
    }
}
