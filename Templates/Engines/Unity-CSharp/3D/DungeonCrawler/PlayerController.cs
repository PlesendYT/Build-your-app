/// <summary>
/// Dungeon crawler player with melee/ranged combat, dodge roll, and item interaction.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f, dodgeSpeed = 15f, dodgeDuration = 0.3f;
    private CharacterController cc;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;
    private bool isDodging;
    private float dodgeTimer;
    private Vector3 dodgeDir;

    [Header("Combat")]
    public int attackDamage = 25;
    public float attackRange = 2f, attackRate = 0.5f;
    public Transform attackOrigin;
    private float nextAttackTime;

    [Header("Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    private void Awake() { cc = GetComponent<CharacterController>(); currentHealth = maxHealth; }

    private void Update()
    {
        if (isDodging) { dodgeTimer -= Time.deltaTime; cc.Move(dodgeDir * dodgeSpeed * Time.deltaTime); if (dodgeTimer <= 0f) isDodging = false; }
        else cc.Move((transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed * Time.deltaTime);
        velocity.y -= gravity * Time.deltaTime; cc.Move(velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    public void OnDodge(InputAction.CallbackContext ctx) { if (!ctx.performed || isDodging) return; isDodging = true; dodgeTimer = dodgeDuration; dodgeDir = moveInput.magnitude > 0.1f ? (transform.right * moveInput.x + transform.forward * moveInput.y).normalized : -transform.forward; }
    public void OnAttack(InputAction.CallbackContext ctx) { if (!ctx.performed || Time.time < nextAttackTime) return; nextAttackTime = Time.time + attackRate; foreach (Collider h in Physics.OverlapSphere(attackOrigin.position, attackRange)) if (h.TryGetComponent<EnemyAI>(out EnemyAI e)) e.TakeDamage(attackDamage); }
    public void TakeDamage(int dmg) { if (isDodging) return; currentHealth -= dmg; if (currentHealth <= 0) { gameObject.SetActive(false); GameManager.Instance.PlayerDied(); } }
}
