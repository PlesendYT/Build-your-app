/// <summary>
/// Controls the metroidvania player with movement, jump, double jump, dash, power-ups, and health.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 13f;
    public float dashForce = 20f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool hasDoubleJump;
    private bool hasDash;
    private bool canDash = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;
    private Animator animator;

    /// <summary>
    /// Caches components and initializes health.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Checks grounded state, resets double jump and dash when grounded, and updates animations.
    /// </summary>
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            canDoubleJump = hasDoubleJump;
            canDash = true;
        }
        UpdateAnimation();
    }

    /// <summary>
    /// Applies horizontal movement velocity.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    /// <summary>
    /// Updates animator parameters for speed, grounded state, and vertical velocity.
    /// </summary>
    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
        }
    }

    /// <summary>
    /// Reads horizontal movement input.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Handles jumping: normal jump when grounded, double jump if unlocked.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        else if (canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = false;
        }
    }

    /// <summary>
    /// Performs a dash in the horizontal movement direction if unlocked.
    /// </summary>
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && hasDash && canDash)
        {
            rb.linearVelocity = new Vector2(moveInput.x * dashForce, 0f);
            canDash = false;
        }
    }

    /// <summary>
    /// Unlocks a power-up ability (double jump or dash).
    /// </summary>
    public void UnlockPowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.DoubleJump:
                hasDoubleJump = true;
                break;
            case PowerUpType.Dash:
                hasDash = true;
                break;
        }
    }

    /// <summary>
    /// Reduces health by damage and triggers death if health reaches zero.
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
    /// Deactivates the player and triggers game over.
    /// </summary>
    private void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool HasDoubleJump() => hasDoubleJump;
    public bool HasDash() => hasDash;
}

/// <summary>
/// Types of power-ups the player can unlock.
/// </summary>
public enum PowerUpType { DoubleJump, Dash, WallJump }
