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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

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

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rb.linearVelocity.y);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && hasDash && canDash)
        {
            rb.linearVelocity = new Vector2(moveInput.x * dashForce, 0f);
            canDash = false;
        }
    }

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
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool HasDoubleJump() => hasDoubleJump;
    public bool HasDash() => hasDash;
}

public enum PowerUpType { DoubleJump, Dash, WallJump }
