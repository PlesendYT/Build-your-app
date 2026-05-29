/// <summary>
/// Fighting game character with movement, combo attacks, jump, and health bar.
/// </summary>
using UnityEngine;
using UnityEngine.InputSystem;

public class FighterController : MonoBehaviour
{
    [Header("Stats")]
    public string fighterName = "Fighter";
    public int maxHealth = 100;
    public int currentHealth;
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Combo")]
    public int maxCombo = 3;
    public float comboWindow = 0.5f;
    private int currentCombo;
    private float lastAttackTime;

    [Header("References")]
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask opponentLayer;
    private CharacterController characterController;
    private Vector2 moveInput;
    private Vector3 velocity;
    private float gravity = 20f;
    private bool isGrounded;
    private bool isAttacking;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;
        characterController.Move(move * Time.deltaTime);
        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (Time.time > lastAttackTime + comboWindow) currentCombo = 0;

        if (animator != null)
        {
            animator.SetFloat("Speed", moveInput.magnitude);
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetInteger("Combo", currentCombo);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking)
            StartCoroutine(AttackRoutine());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
            velocity.y = jumpForce;
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator?.SetTrigger("Attack");

        int combo = Mathf.Min(currentCombo, maxCombo - 1);
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, opponentLayer);
        foreach (Collider hit in hits)
        {
            FighterController opponent = hit.GetComponent<FighterController>();
            if (opponent != null)
            {
                int damage = 10 + combo * 5;
                opponent.TakeDamage(damage);
            }
        }

        currentCombo = (currentCombo + 1) % maxCombo;
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        animator?.SetTrigger("Hit");
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        animator?.SetTrigger("Death");
        enabled = false;
        GameManager.Instance?.RoundOver();
    }
}
