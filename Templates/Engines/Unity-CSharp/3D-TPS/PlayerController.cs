using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;
    public float rotationSpeed = 10f;
    private Vector2 moveInput;
    private Rigidbody rb;
    private Animator animator;

    [Header("Combat")]
    public Weapon weapon;
    public Transform aimTarget;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
        UpdateAnimation();
    }

    private void HandleMovement()
    {
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = camForward * moveInput.y + camRight * moveInput.x;
        float speed = moveInput.sqrMagnitude > 0.1f ? sprintSpeed : moveSpeed;

        rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);
    }

    private void HandleRotation()
    {
        Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        if (direction.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", rb.linearVelocity.magnitude / sprintSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && weapon != null)
            weapon.OnFire(context);
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
