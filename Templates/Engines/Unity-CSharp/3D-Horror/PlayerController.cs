using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float stamina = 100f;
    public float staminaDrain = 20f;
    public float staminaRegen = 10f;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float currentStamina;
    private bool isRunning;
    private float gravity = 20f;

    [Header("Look")]
    public float sensitivity = 2f;
    public float maxLookAngle = 80f;
    public Transform cameraTransform;
    private float verticalRotation;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentStamina = stamina;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleStamina();
    }

    private void HandleMovement()
    {
        isRunning = moveInput.sqrMagnitude > 0.1f && currentStamina > 0 && Keyboard.current.leftShiftKey.isPressed;

        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * speed;
        characterController.Move(move * Time.deltaTime);

        if (!characterController.isGrounded)
        {
            velocity.y -= gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
    }

    private void HandleLook()
    {
        Vector2 look = Mouse.current.delta.ReadValue() * sensitivity * 0.01f;
        transform.Rotate(Vector3.up * look.x);
        verticalRotation = Mathf.Clamp(verticalRotation - look.y, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleStamina()
    {
        if (isRunning)
            currentStamina = Mathf.Max(0, currentStamina - staminaDrain * Time.deltaTime);
        else
            currentStamina = Mathf.Min(stamina, currentStamina + staminaRegen * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void TakeDamage(int damage)
    {
        GameManager.Instance.AddFear(damage);
    }

    public float GetStamina() => currentStamina;
    public float GetMaxStamina() => stamina;
}
