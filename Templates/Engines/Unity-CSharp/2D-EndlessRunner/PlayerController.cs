/// <summary>
/// Controls the endless runner player with forward movement, jumping, sliding, power-ups, and collision handling.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float forwardSpeed = 8f;
    public float jumpForce = 12f;
    public float slideDuration = 1f;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isSliding;
    private bool hasShield;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Visual")]
    public Animator animator;
    public Transform slideCollider;

    /// <summary>
    /// Caches the rigidbody component.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Checks if the player is grounded and updates animation each frame.
    /// </summary>
    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        UpdateAnimation();
    }

    /// <summary>
    /// Applies forward movement velocity when not sliding.
    /// </summary>
    private void FixedUpdate()
    {
        if (!isSliding)
        {
            rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);
        }
    }

    /// <summary>
    /// Updates animator parameters for grounded and sliding states.
    /// </summary>
    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsSliding", isSliding);
        }
    }

    /// <summary>
    /// Makes the player jump if grounded and not sliding.
    /// </summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    /// <summary>
    /// Starts the slide coroutine when the slide action is performed.
    /// </summary>
    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isSliding)
        {
            StartCoroutine(SlideRoutine());
        }
    }

    /// <summary>
    /// Disables the slide collider for the duration of the slide, then re-enables it.
    /// </summary>
    private System.Collections.IEnumerator SlideRoutine()
    {
        isSliding = true;
        if (slideCollider != null)
            slideCollider.gameObject.SetActive(false);

        yield return new WaitForSeconds(slideDuration);

        if (slideCollider != null)
            slideCollider.gameObject.SetActive(true);
        isSliding = false;
    }

    /// <summary>
    /// Handles collisions: obstacles (destroyed by shield or kill player), collectibles, and power-ups.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (hasShield)
            {
                hasShield = false;
                Destroy(other.gameObject);
            }
            else
            {
                Die();
            }
        }
        else if (other.CompareTag("Collectible"))
        {
            GameManager.Instance.AddScore(100);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUp"))
        {
            PowerUp powerUp = other.GetComponent<PowerUp>();
            if (powerUp != null)
                powerUp.Apply(this);
            Destroy(other.gameObject);
        }
    }

    /// <summary>
    /// Enables or disables the shield.
    /// </summary>
    public void SetShield(bool active) => hasShield = active;

    /// <summary>
    /// Kills the player: stops movement, deactivates, and triggers game over.
    /// </summary>
    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }
}
