using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sneakSpeed = 2f;
    public float runSpeed = 7f;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;
    private bool isGrounded;
    private bool isSneaking;

    [Header("Look")]
    public float sensitivity = 2f;
    public float maxLookAngle = 80f;
    public Transform cameraTransform;
    private float verticalRotation;

    [Header("Stealth")]
    public float visibility = 1f;
    public float noiseLevel = 1f;
    public bool isInShadow;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        UpdateStealth();
    }

    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        float speed = isSneaking ? sneakSpeed : (moveInput.sqrMagnitude > 0.1f ? runSpeed : walkSpeed);
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

    private void UpdateStealth()
    {
        noiseLevel = isSneaking ? 0.2f : (moveInput.sqrMagnitude > 0.1f ? 0.8f : 0f);
        isInShadow = !Physics.Raycast(transform.position + Vector3.up, Vector3.up, 10f);
        visibility = isInShadow ? 0.3f : 1f;
        if (isSneaking) visibility *= 0.5f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnSneak(InputAction.CallbackContext context)
    {
        if (context.performed) isSneaking = !isSneaking;
    }

    public float GetVisibility() => visibility;
    public float GetNoise() => noiseLevel;
    public bool IsSneaking() => isSneaking;
}
