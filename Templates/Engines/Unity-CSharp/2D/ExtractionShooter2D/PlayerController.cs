/// <summary>
/// Controls player movement, shooting, looting, and health management in the extraction shooter.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;
    private float fireTimer;
    private Vector2 aimDirection = Vector2.right;

    [Header("Interaction")]
    public float lootRange = 2f;
    public LayerMask lootLayer;

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
    /// Applies movement and handles shooting cooldown each frame.
    /// </summary>
    private void Update()
    {
        if (gameManager.isExtracted) return;
        fireTimer -= Time.deltaTime;
        if (aimDirection != Vector2.zero)
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg);
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

    public void OnAim(InputAction.CallbackContext context)
    {
        Vector2 v = context.ReadValue<Vector2>();
        if (v != Vector2.zero) aimDirection = v.normalized;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed || gameManager.isExtracted || fireTimer > 0) return;
        fireTimer = fireRate;
        GameObject b = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        b.GetComponent<Rigidbody2D>().linearVelocity = aimDirection * 15f;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        Collider2D hit = Physics2D.OverlapCircle(transform.position, lootRange, lootLayer);
        if (hit != null && hit.TryGetComponent(out Lootable l) && gameManager.AddLoot(l.itemName, l.value, l.quantity))
            Destroy(hit.gameObject);
    }

    public void OnHeal(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        var m = gameManager.lootInventory.Find(i => i.itemName == "Medkit");
        if (m != null) { gameManager.Heal(40f); m.quantity--; if (m.quantity <= 0) gameManager.lootInventory.Remove(m); }
    }
}
