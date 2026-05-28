using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float boostSpeed = 10f;
    public float dodgeCooldown = 1f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private float dodgeTimer;

    [Header("Boundary")]
    public Vector2 minBounds;
    public Vector2 maxBounds;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;
    public GameObject deathEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (dodgeTimer > 0f)
            dodgeTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        float speed = dodgeTimer > 0f ? boostSpeed : moveSpeed;
        Vector2 velocity = moveInput * speed;
        rb.linearVelocity = velocity;
        Vector2 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.y = Mathf.Clamp(pos.y, minBounds.y, maxBounds.y);
        transform.position = pos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnDodge(InputAction.CallbackContext context)
    {
        if (context.performed && dodgeTimer <= 0f)
        {
            dodgeTimer = dodgeCooldown;
        }
    }

    public void TakeDamage()
    {
        currentHealth--;
        if (currentHealth <= 0)
        {
            if (deathEffect != null)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }

    public int GetHealth() => currentHealth;
}
