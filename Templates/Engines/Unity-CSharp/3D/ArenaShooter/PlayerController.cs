/// <summary>
/// Arena shooter player with fast movement, dash, weapon switching, and power-up effects.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f, dashSpeed = 20f, dashDuration = 0.2f;
    private CharacterController cc;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;
    private bool isDashing;
    private float dashTimer;

    [Header("Weapons")]
    public GameObject[] weaponPrefabs;
    public int currentWeaponIndex;
    private WeaponSystem currentWeapon;

    [Header("Power-Ups")]
    public float damageMultiplier = 1f, speedMultiplier = 1f, powerUpDuration;
    private bool hasPowerUp;

    [Header("Combat")]
    public int maxHealth = 100;
    private int currentHealth;
    public Transform shootOrigin;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        EquipWeapon(0);
    }

    private void Update()
    {
        if (isDashing) { HandleDash(); } else { HandleMovement(); }
        ApplyGravity();
        if (hasPowerUp)
        {
            powerUpDuration -= Time.deltaTime;
            if (powerUpDuration <= 0f) RemovePowerUp();
        }
        if (currentWeapon != null && GameManager.Instance.isWaveActive && Mouse.current.leftButton.isPressed)
            currentWeapon.TryShoot();
    }

    private void HandleMovement()
    {
        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed * speedMultiplier;
        cc.Move(move * Time.deltaTime);
    }

    private void HandleDash()
    {
        dashTimer -= Time.deltaTime;
        Vector3 dir = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
        if (dir.magnitude < 0.1f) dir = transform.forward;
        cc.Move(dir * dashSpeed * Time.deltaTime);
        if (dashTimer <= 0f) isDashing = false;
    }

    private void ApplyGravity()
    {
        velocity.y -= gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isDashing) { isDashing = true; dashTimer = dashDuration; }
    }

    public void OnSwitchWeapon(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        float scroll = ctx.ReadValue<float>();
        EquipWeapon((currentWeaponIndex + (scroll > 0 ? 1 : -1) + weaponPrefabs.Length) % weaponPrefabs.Length);
    }

    private void EquipWeapon(int index)
    {
        if (currentWeapon != null) Destroy(currentWeapon.gameObject);
        currentWeaponIndex = index;
        currentWeapon = Instantiate(weaponPrefabs[index], shootOrigin).GetComponent<WeaponSystem>();
    }

    public void ApplyPowerUp(float dmgMult, float spdMult, float dur)
    {
        damageMultiplier = dmgMult; speedMultiplier = spdMult; powerUpDuration = dur; hasPowerUp = true;
    }

    private void RemovePowerUp() { damageMultiplier = 1f; speedMultiplier = 1f; hasPowerUp = false; }

    public void TakeDamage(int damage)
    {
        if (isDashing) return;
        currentHealth -= damage;
        if (currentHealth <= 0) { gameObject.SetActive(false); GameManager.Instance.PlayerDied(); }
    }

    public void Respawn() { currentHealth = maxHealth; transform.position = Vector3.zero; }
}
