using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpForce = 8f;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;
    private bool isGrounded;

    [Header("Look")]
    public float sensitivity = 2f;
    public float maxLookAngle = 80f;
    public Transform cameraTransform;
    private float verticalRotation;

    [Header("Stats")]
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
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float speed = moveInput.sqrMagnitude > 0.1f ? runSpeed : walkSpeed;
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * speed;
        characterController.Move(move * Time.deltaTime);

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        Vector2 look = Mouse.current.delta.ReadValue() * sensitivity * 0.01f;
        transform.Rotate(Vector3.up * look.x);
        verticalRotation -= look.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
            velocity.y = jumpForce;
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
}
