using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;

    [Header("Survival Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float maxHunger = 100f;
    public float currentHunger;
    public float maxThirst = 100f;
    public float currentThirst;
    public float hungerDrain = 2f;
    public float thirstDrain = 3f;
    public float healthRegen = 1f;

    [Header("Look")]
    public float sensitivity = 2f;
    public float maxLookAngle = 80f;
    public Transform cameraTransform;
    private float verticalRotation;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        currentThirst = maxThirst;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
        HandleLook();
        HandleSurvival();
    }

    private void HandleMovement()
    {
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y);
        float speed = moveInput.sqrMagnitude > 0.1f ? walkSpeed : 0f;
        characterController.Move(move * speed * Time.deltaTime);
        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void HandleLook()
    {
        Vector2 look = Mouse.current.delta.ReadValue() * sensitivity * 0.01f;
        transform.Rotate(Vector3.up * look.x);
        verticalRotation = Mathf.Clamp(verticalRotation - look.y, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    private void HandleSurvival()
    {
        currentHunger = Mathf.Max(0, currentHunger - hungerDrain * Time.deltaTime);
        currentThirst = Mathf.Max(0, currentThirst - thirstDrain * Time.deltaTime);

        if (currentHunger <= 0 || currentThirst <= 0)
        {
            currentHealth -= 5 * Time.deltaTime;
        }
        else if (currentHealth < maxHealth)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + healthRegen * Time.deltaTime);
        }

        if (currentHealth <= 0) Die();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Eat(float hungerValue)
    {
        currentHunger = Mathf.Min(maxHunger, currentHunger + hungerValue);
    }

    public void Drink(float thirstValue)
    {
        currentThirst = Mathf.Min(maxThirst, currentThirst + thirstValue);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }

    public float GetHealthPercent() => currentHealth / maxHealth;
    public float GetHungerPercent() => currentHunger / maxHunger;
    public float GetThirstPercent() => currentThirst / maxThirst;
}
