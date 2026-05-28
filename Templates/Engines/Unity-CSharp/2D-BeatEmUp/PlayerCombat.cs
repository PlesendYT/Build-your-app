using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combo System")]
    public int maxCombo = 3;
    public float comboResetTime = 1f;
    private int currentCombo;
    private float lastAttackTime;
    private bool isAttacking;

    [Header("Attack")]
    public int[] comboDamages = { 1, 2, 3 };
    public float[] comboRanges = { 1.5f, 1.8f, 2f };
    public float attackCooldown = 0.3f;
    public Transform attackPoint;
    public LayerMask enemyLayer;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Time.time > lastAttackTime + comboResetTime)
        {
            currentCombo = 0;
        }
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }
    }

    private void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));
            animator.SetBool("IsAttacking", isAttacking);
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
        {
            StartCoroutine(PerformAttack());
        }
    }

    private System.Collections.IEnumerator PerformAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        int combo = Mathf.Min(currentCombo, maxCombo - 1);
        float range = comboRanges[combo];
        int damage = comboDamages[combo];

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, range, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            EnemyController enemy = hit.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        currentCombo = (currentCombo + 1) % maxCombo;
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, comboRanges[0]);
    }
}
