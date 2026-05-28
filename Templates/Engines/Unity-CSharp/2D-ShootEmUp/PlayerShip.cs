using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public Vector2 moveBounds = new Vector2(8f, 5f);
    private Vector2 moveInput;
    private Rigidbody2D rb;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform[] firePoints;
    public float fireRate = 0.15f;
    private float lastFireTime;
    public int weaponLevel = 1;

    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, -moveBounds.x, moveBounds.x);
        clamped.y = Mathf.Clamp(clamped.y, -moveBounds.y, moveBounds.y);
        transform.position = clamped;
    }

    private void Fire()
    {
        if (bulletPrefab == null) return;

        int pointsToUse = Mathf.Min(weaponLevel, firePoints.Length);
        for (int i = 0; i < pointsToUse; i++)
        {
            Instantiate(bulletPrefab, firePoints[i].position, firePoints[i].rotation);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void UpgradeWeapon()
    {
        weaponLevel = Mathf.Min(weaponLevel + 1, firePoints.Length);
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
