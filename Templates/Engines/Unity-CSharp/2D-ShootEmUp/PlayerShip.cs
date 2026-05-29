/// <summary>
/// Controls the player ship in a shoot-em-up with movement, auto-fire, weapon upgrades, and health.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public Vector2 moveBounds = new Vector2(8f, 5f);
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float fireRate = 0.15f;
    private float lastFireTime;
    public int weaponLevel = 1;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    /// <summary>
    /// Caches the rigidbody and initializes health.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Auto-fires at the configured rate each frame.
    /// </summary>
    private void Update()
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    /// <summary>
    /// Moves the ship based on input and clamps to bounds.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, -moveBounds.x, moveBounds.x);
        clamped.y = Mathf.Clamp(clamped.y, -moveBounds.y, moveBounds.y);
        transform.position = clamped;
    }

    /// <summary>
    /// Fires bullets from all fire points up to the current weapon level.
    /// </summary>
    private void Fire()
    {
        if (bulletPrefab == null) return;

        int pointsToUse = Mathf.Min(weaponLevel, firePoints.Length);
        for (int i = 0; i < pointsToUse; i++)
        {
            Instantiate(bulletPrefab, firePoints[i].position, firePoints[i].rotation);
        }
    }

    /// <summary>
    /// Reads movement input from the input system.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Increases the weapon level up to the number of available fire points.
    /// </summary>
    public void UpgradeWeapon()
    {
        weaponLevel = Mathf.Min(weaponLevel + 1, firePoints.Length);
    }

    /// <summary>
    /// Reduces health and triggers death if health reaches zero.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Disables the ship and triggers game over.
    /// </summary>
    private void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
