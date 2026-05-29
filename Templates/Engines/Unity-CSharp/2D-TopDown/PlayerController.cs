/// <summary>
/// Controls the top-down player with movement, animation, interaction, and health.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;

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
    /// Applies movement velocity and updates animation each physics frame.
    /// </summary>
    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        UpdateAnimation();
    }

    /// <summary>
    /// Updates the animator parameters for movement direction and speed.
    /// </summary>
    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("MoveX", moveInput.x);
            animator.SetFloat("MoveY", moveInput.y);
            animator.SetFloat("Speed", moveInput.sqrMagnitude);
        }
    }

    /// <summary>
    /// Reads normalized movement input from the input system.
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
    }

    /// <summary>
    /// Performs a raycast in the move direction to interact with IInteractable objects.
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, moveInput, 1f);
            if (hit.collider != null && hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }
    }

    /// <summary>
    /// Reduces health and triggers game over if health reaches zero.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }

    /// <summary>
    /// Heals the player up to the maximum health.
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}

/// <summary>
/// Interface for objects the player can interact with.
/// </summary>
public interface IInteractable
{
    void Interact();
}
