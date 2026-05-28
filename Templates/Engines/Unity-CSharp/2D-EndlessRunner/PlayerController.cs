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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (!isSliding)
        {
            rb.linearVelocity = new Vector2(forwardSpeed, rb.linearVelocity.y);
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsSliding", isSliding);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && !isSliding)
        {
            StartCoroutine(SlideRoutine());
        }
    }

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

    public void SetShield(bool active) => hasShield = active;

    private void Die()
    {
        rb.linearVelocity = Vector2.zero;
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }
}
