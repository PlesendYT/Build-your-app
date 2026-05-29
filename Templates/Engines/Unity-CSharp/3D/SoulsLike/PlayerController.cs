/// <summary>
/// Souls-like player with lock-on combat, dodge roll, parry, and stamina management.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f, runSpeed = 7f, rollSpeed = 12f;
    private CharacterController cc;
    private Vector2 moveInput, velocity;
    private float gravity = 25f;

    [Header("Stamina")]
    public float stamina = 100f, maxStamina = 100f, staminaRegen = 15f;
    public float rollCost = 25f, attackCost = 20f, regenDelay = 1f;
    private float regenTimer;
    private bool isRolling;
    private float rollTimer;
    private Vector3 rollDir;

    [Header("Combat")]
    public int attackDamage = 40;
    public float attackRange = 2.5f;
    public Transform attackOrigin;

    [Header("Lock-On")]
    public float lockOnRange = 20f;
    public Transform lockOnTarget;

    private void Awake() { cc = GetComponent<CharacterController>(); stamina = maxStamina; }

    private void Update()
    {
        if (isRolling) { rollTimer -= Time.deltaTime; cc.Move(rollDir * rollSpeed * Time.deltaTime); if (rollTimer <= 0f) isRolling = false; }
        else
        {
            float speed = moveInput.sqrMagnitude > 0.1f ? runSpeed : walkSpeed;
            cc.Move((transform.right * moveInput.x + transform.forward * moveInput.y) * speed * Time.deltaTime);
            if (stamina < maxStamina) { regenTimer += Time.deltaTime; if (regenTimer >= regenDelay) stamina = Mathf.Min(maxStamina, stamina + staminaRegen * Time.deltaTime); }
        }
        velocity.y -= gravity * Time.deltaTime; cc.Move(velocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (lockOnTarget == null) return;
        Vector3 d = lockOnTarget.position - transform.position; d.y = 0f;
        if (d.magnitude > 0.1f) transform.rotation = Quaternion.LookRotation(d);
    }

    public void OnMove(InputAction.CallbackContext ctx) { moveInput = ctx.ReadValue<Vector2>(); regenTimer = 0f; }
    public void OnRoll(InputAction.CallbackContext ctx) { if (!ctx.performed || isRolling || stamina < rollCost) return; stamina -= rollCost; regenTimer = 0f; isRolling = true; rollTimer = 0.4f; rollDir = moveInput.magnitude > 0.1f ? (transform.right * moveInput.x + transform.forward * moveInput.y).normalized : -transform.forward; }
    public void OnAttack(InputAction.CallbackContext ctx) { if (!ctx.performed || isRolling || stamina < attackCost) return; stamina -= attackCost; regenTimer = 0f; foreach (Collider h in Physics.OverlapSphere(attackOrigin.position, attackRange)) if (h.TryGetComponent<EnemyAI>(out EnemyAI e)) e.TakeDamage(attackDamage); }
    public void OnLockOn(InputAction.CallbackContext ctx) { if (!ctx.performed) return; if (lockOnTarget != null) { lockOnTarget = null; return; } float closest = Mathf.Infinity; foreach (Collider c in Physics.OverlapSphere(transform.position, lockOnRange)) if (c.TryGetComponent<EnemyAI>(out _)) { float d = Vector3.Distance(transform.position, c.transform.position); if (d < closest) { closest = d; lockOnTarget = c.transform; } } }
    public void OnParry(InputAction.CallbackContext ctx) { if (!ctx.performed) return; foreach (Collider h in Physics.OverlapSphere(attackOrigin.position, attackRange * 0.7f)) if (h.TryGetComponent<EnemyAI>(out EnemyAI e) && e.Parried()) { e.Stun(2f); e.TakeDamage(attackDamage * 2); } }
    public void TakeDamage(int dmg) { if (isRolling) return; GameManager.Instance.health -= dmg; if (GameManager.Instance.health <= 0) { gameObject.SetActive(false); GameManager.Instance.PlayerDied(); } }
    public void RestoreStats() { GameManager.Instance.health = GameManager.Instance.maxHealth; GameManager.Instance.stamina = GameManager.Instance.maxStamina; stamina = maxStamina; }
}
