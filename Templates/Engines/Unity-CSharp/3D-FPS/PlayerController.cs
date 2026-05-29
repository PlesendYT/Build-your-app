/// <summary>
/// FPS player with movement, mouse look, jumping, and gravity.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 8f;
    public float gravity = 20f;
    private float currentSpeed;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private CharacterController characterController;

    [Header("Look")]
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;
    private float verticalRotation;
    public Transform cameraTransform;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
    }

    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        currentSpeed = moveInput.sqrMagnitude > 0.1f ? runSpeed : walkSpeed;
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * currentSpeed;
        characterController.Move(move * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        Vector2 lookDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * 0.01f;
        transform.Rotate(Vector3.up * lookDelta.x);

        verticalRotation -= lookDelta.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            velocity.y = jumpForce;
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
}
