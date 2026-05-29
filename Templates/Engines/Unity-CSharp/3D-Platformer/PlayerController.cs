/// <summary>
/// 3D platformer character with movement, jumping, double jump, and ground detection.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 10f;
    public float gravity = 30f;
    private CharacterController characterController;
    private Vector3 velocity;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool hasDoubleJump;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.4f;
    public LayerMask groundLayer;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded)
        {
            canDoubleJump = hasDoubleJump;
            if (velocity.y < 0) velocity.y = -2f;
        }

        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;
        characterController.Move(move * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
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
            velocity.y = jumpForce;
        }
        else if (canDoubleJump)
        {
            velocity.y = jumpForce;
            canDoubleJump = false;
        }
    }

    public void UnlockDoubleJump()
    {
        hasDoubleJump = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
