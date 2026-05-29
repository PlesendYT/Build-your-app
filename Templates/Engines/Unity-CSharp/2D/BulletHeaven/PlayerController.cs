/// <summary>
/// Controls the bullet heaven player: movement with auto-aim and auto-attack.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Auto-Attack")]
    public Weapon equippedWeapon;
    public float attackRadius = 8f;
    public LayerMask enemyLayer;

    [Header("References")]
    public GameManager gameManager;

    /// <summary>
    /// Caches component references.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Handles auto-aim and auto-attack each frame.
    /// </summary>
    private void Update()
    {
        if (equippedWeapon == null) return;

        // Find the nearest enemy within attack radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);
        Transform target = null;
        float closestDist = float.MaxValue;

        foreach (Collider2D enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                target = enemy.transform;
            }
        }

        // Auto-attack the nearest target
        if (target != null)
        {
            equippedWeapon.TryAttack(transform.position, target);
        }
    }

    /// <summary>
    /// Applies movement velocity each physics frame.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    /// <summary>
    /// Reads movement input from the Input System.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context) { moveInput = context.ReadValue<Vector2>(); }

    /// <summary>
    /// Collects an XP gem dropped by a defeated enemy.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("XP")) return;
        XPGem gem = other.GetComponent<XPGem>();
        if (gem != null) { gameManager.AddXP(gem.xpAmount); Destroy(other.gameObject); }
    }

    /// <summary>
    /// Equips a new weapon or upgrades the current one.
    /// </summary>
    public void EquipWeapon(Weapon newWeapon)
    {
        if (equippedWeapon == null) equippedWeapon = Instantiate(newWeapon, transform);
        else equippedWeapon.Upgrade();
        Debug.Log("Weapon equipped/upgraded.");
    }
}
