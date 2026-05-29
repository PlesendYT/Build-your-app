/// <summary>
/// FPS player with movement, shooting, health, and loot carrying for extraction gameplay.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f, sprintSpeed = 8f, jumpForce = 8f, gravity = 20f;
    private CharacterController cc;
    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;

    [Header("Look")]
    public float sensitivity = 2f, maxLookAngle = 80f;
    public Transform cameraTransform;
    private float verticalRot;

    [Header("Combat")]
    public int maxHealth = 100;
    private int currentHealth;
    public int damage = 25;
    public float fireRate = 0.1f, range = 100f;
    private float nextFireTime;
    public Transform shootOrigin;

    [Header("Loot")]
    public int carriedLootValue, maxCarryWeight = 50, currentWeight;

    private void Awake() { cc = GetComponent<CharacterController>(); currentHealth = maxHealth; Cursor.lockState = CursorLockMode.Locked; }

    private void Update()
    {
        isGrounded = cc.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;
        float speed = moveInput.sqrMagnitude > 0.1f && isGrounded ? sprintSpeed : walkSpeed;
        cc.Move((transform.right * moveInput.x + transform.forward * moveInput.y) * speed * Time.deltaTime);
        velocity.y -= gravity * Time.deltaTime; cc.Move(velocity * Time.deltaTime);
        Vector2 look = Mouse.current.delta.ReadValue() * sensitivity * 0.01f;
        transform.Rotate(Vector3.up * look.x);
        verticalRot = Mathf.Clamp(verticalRot - look.y, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(verticalRot, 0, 0);
    }

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    public void OnJump(InputAction.CallbackContext ctx) { if (ctx.performed && isGrounded) velocity.y = jumpForce; }
    public void OnShoot(InputAction.CallbackContext ctx) { if (ctx.performed && Time.time >= nextFireTime) { nextFireTime = Time.time + fireRate; if (Physics.Raycast(shootOrigin.position, shootOrigin.forward, out RaycastHit h, range) && h.collider.TryGetComponent<EnemyAI>(out EnemyAI e)) e.TakeDamage(damage); } }
    public void AddLoot(int val, int w) { if (currentWeight + w > maxCarryWeight) return; carriedLootValue += val; currentWeight += w; }
    public void TakeDamage(int dmg) { currentHealth -= dmg; if (currentHealth <= 0) { gameObject.SetActive(false); GameManager.Instance.PlayerDied(); } }
}
