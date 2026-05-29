/// <summary>
/// Aggressive souls-like enemy with attack patterns, combos, and parryable attacks.
/// </summary>
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Combat")]
    public int damage = 20, maxHealth = 150, currentHealth;
    public float detectionRange = 15f, attackRange = 2.5f, moveSpeed = 3.5f;
    private Transform player;

    [Header("Attack Patterns")]
    public string[] attackPatterns = { "Slash", "Stab", "Slam" };
    public float[] patternWindup = { 0.5f, 0.8f, 1.2f };
    public float comboChance = 0.3f;
    private bool isAttacking, isStunned, isParryWindowOpen, hasBeenParried;
    private float stunTimer;

    public enum BossState { Idle, Chase, Attack, Stunned }
    public BossState state = BossState.Idle;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;
        if (isStunned) { stunTimer -= Time.deltaTime; if (stunTimer <= 0f) { isStunned = false; state = BossState.Chase; } return; }

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange && !isAttacking) { state = BossState.Attack; StartAttack(); }
        else if (dist <= detectionRange) { state = BossState.Chase; MoveTowards(player.position, moveSpeed); }
        else if (state == BossState.Chase) state = BossState.Idle;
    }

    private void StartAttack()
    {
        isAttacking = true;
        isParryWindowOpen = true;
        int idx = Random.Range(0, attackPatterns.Length);
        Invoke(nameof(ExecuteAttack), patternWindup[idx]);
    }

    private void ExecuteAttack()
    {
        isParryWindowOpen = false;
        if (hasBeenParried) { hasBeenParried = false; isAttacking = false; state = BossState.Stunned; return; }

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
            player.GetComponent<PlayerController>()?.TakeDamage(damage);

        if (Random.value < comboChance) Invoke(nameof(StartAttack), 0.3f);
        else { isAttacking = false; state = BossState.Chase; }
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 dir = (target - transform.position).normalized; dir.y = 0f;
        transform.position += dir * speed * Time.deltaTime;
        if (dir.magnitude > 0.1f) transform.rotation = Quaternion.LookRotation(dir);
    }

    public bool Parried()
    {
        if (!isParryWindowOpen) return false;
        hasBeenParried = true;
        return true;
    }

    public void Stun(float duration) { isStunned = true; stunTimer = duration; state = BossState.Stunned; isAttacking = false; }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) { GameManager.Instance.AddSouls(Random.Range(500, 2000)); GameManager.Instance.BossDefeated(); Destroy(gameObject); }
        else state = BossState.Chase;
    }
}
