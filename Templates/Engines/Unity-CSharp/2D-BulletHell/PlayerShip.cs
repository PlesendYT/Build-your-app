/// <summary>
/// Controls player ship movement with speed boost/dodge, boundary clamping, and health management.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float boostSpeed = 10f;
    public float dodgeCooldown = 1f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private float dodgeTimer;

    [Header("Boundary")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    public GameObject deathEffect;

    /// <summary>
    /// Caches the rigidbody and initializes health.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Counts down the dodge timer each frame.
    /// </summary>
    private void Update()
    {
        if (dodgeTimer > 0f)
            dodgeTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Applies movement velocity with optional speed boost and clamps position to boundaries.
    /// </summary>
    private void FixedUpdate()
    {
        float speed = dodgeTimer > 0f ? boostSpeed : moveSpeed;
        Vector2 velocity = moveInput * speed;
        rb.linearVelocity = velocity;
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        transform.position = pos;
    }

    /// <summary>
    /// Reads movement input from the Input System.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Activates the dodge speed boost if the cooldown has elapsed.
    /// </summary>
    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed && dodgeTimer <= 0f)
        {
            dodgeTimer = dodgeCooldown;
        }
    }

    /// <summary>
    /// Reduces health on damage; triggers game over and death effect when health reaches zero.
    /// </summary>
    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }

    /// <summary>
    /// Returns the current health value.
    /// </summary>
    public int GetHealth() => currentHealth;
}
