/// <summary>
/// RTS unit with NavMesh movement, attack, health; includes Building and GameManager classes.
/// </summary>
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Unit : MonoBehaviour
{
    [Header("Stats")]
    public string unitName;
    public int maxHealth = 50;
    public int currentHealth;
    public int damage = 10;
    public float attackRange = 5f;
    public float attackCooldown = 1f;

    private NavMeshAgent agent;
    private Animator animator;
    private float lastAttackTime;
    public bool isSelected;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void Attack(Unit target)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            transform.LookAt(target.transform);
            target.TakeDamage(damage);
            lastAttackTime = Time.time;
            animator?.SetTrigger("Attack");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }
}

public class Building : MonoBehaviour
{
    public string buildingName;
    public int maxHealth = 100;
    public int currentHealth;
    public GameObject spawnEffect;

    private void Awake() { currentHealth = maxHealth; }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Destroy(gameObject);
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int gold = 200;
    public int wood = 200;

    private void Awake() { Instance = this; }

    public bool SpendResources(int goldCost, int woodCost)
    {
        if (gold >= goldCost && wood >= woodCost)
        {
            gold -= goldCost;
            wood -= woodCost;
            return true;
        }
        return false;
    }
}
