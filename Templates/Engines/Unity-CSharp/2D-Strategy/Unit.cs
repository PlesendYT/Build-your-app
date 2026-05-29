/// <summary>
/// A turn-based strategy unit with grid movement, combat, health, and action tracking.
/// </summary>
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public string unitName;
    public int maxHealth = 10;
    public int currentHealth;
    public int attack = 3;
    public int defense = 1;
    public int moveRange = 3;
    public int attackRange = 1;

    [Header("State")]
    public bool hasMoved;
    public bool hasAttacked;
    public bool isPlayerUnit = true;

    private Vector2Int gridPosition;
    private GridManager gridManager;
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Initializes health and caches component references.
    /// </summary>
    private void Awake()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridManager = FindAnyObjectByType<GridManager>();
    }

    /// <summary>
    /// Snaps to the grid on start.
    /// </summary>
    private void Start()
    {
        gridPosition = gridManager.WorldToGrid(transform.position);
        AlignToGrid();
    }

    /// <summary>
    /// Snaps the unit to its grid position in world space.
    /// </summary>
    public void AlignToGrid()
    {
        transform.position = gridManager.GridToWorld(gridPosition.x, gridPosition.y);
    }

    /// <summary>
    /// Moves the unit to a target grid position within its move range.
    /// </summary>
    public bool MoveTo(Vector2Int target)
    {
        if (!gridManager.IsWalkable(target.x, target.y)) return false;

        int distance = Mathf.Abs(target.x - gridPosition.x) + Mathf.Abs(target.y - gridPosition.y);
        if (distance > moveRange) return false;

        gridPosition = target;
        AlignToGrid();
        hasMoved = true;
        return true;
    }

    /// <summary>
    /// Attacks a target unit within attack range, dealing damage based on attack and defense.
    /// </summary>
    public bool Attack(Unit target)
    {
        if (hasAttacked) return false;

        int distance = Mathf.Abs(target.gridPosition.x - gridPosition.x) +
                       Mathf.Abs(target.gridPosition.y - gridPosition.y);
        if (distance > attackRange) return false;

        int damage = Mathf.Max(1, attack - target.defense);
        target.TakeDamage(damage);
        hasAttacked = true;
        return true;
    }

    /// <summary>
    /// Reduces health and destroys the unit if health reaches zero.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Resets movement and attack flags for the next turn.
    /// </summary>
    public void ResetTurn()
    {
        hasMoved = false;
        hasAttacked = false;
    }

    public Vector2Int GetGridPosition() => gridPosition;
    public bool IsAlive() => currentHealth > 0;
}
