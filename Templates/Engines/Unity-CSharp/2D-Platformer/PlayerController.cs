/// <summary>
/// Controls the 2D platformer player with movement, jumping, ground detection, and health.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

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
    /// Checks if the player is standing on ground each frame.
    /// </summary>
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    /// <summary>
    /// Applies horizontal movement velocity each physics frame.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    /// <summary>
    /// Reads the movement input from the input system.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Makes the player jump if grounded and the jump button was pressed.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
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
    /// Disables the player and triggers game over.
    /// </summary>
    private void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
