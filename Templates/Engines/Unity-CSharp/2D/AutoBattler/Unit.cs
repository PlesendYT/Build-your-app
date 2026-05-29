/// <summary>
/// Represents a unit in the auto-battler with stats, abilities, and combat logic.
/// </summary>
using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Unit Info")]
    public string unitName = "Unit";
    public int cost = 1;
    public Sprite unitIcon;

    [Header("Stats")]
    public int attackPower = 5;
    public int maxHealth = 20;
    public int currentHealth;
    public float attackSpeed = 1f;
    public int boardPosition;

    [Header("Abilities")]
    public string abilityName;
    public float abilityCooldown;
    private float abilityTimer;

    /// <summary>
    /// Initializes the unit's health to maximum.
    /// </summary>
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Applies damage to this unit, triggering death if health drops to zero.
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Handles unit death — triggers removal from the board.
    /// </summary>
    private void Die()
    {
        Debug.Log(unitName + " has been defeated.");
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns the unit's remaining health as a fraction of max.
    /// </summary>
    public float GetHealthPercent()
    {
        return (float)currentHealth / maxHealth;
    }

    /// <summary>
    /// Activates the unit's special ability if it's off cooldown.
    /// </summary>
    public bool UseAbility()
    {
        if (abilityTimer > 0 || string.IsNullOrEmpty(abilityName))
            return false;

        Debug.Log(unitName + " uses " + abilityName);
        abilityTimer = abilityCooldown;
        return true;
    }

    /// <summary>
    /// Counts down the ability cooldown timer each frame.
    /// </summary>
    private void Update()
    {
        if (abilityTimer > 0)
            abilityTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Merges two of the same unit to create a higher-level version.
    /// </summary>
    public void Merge(Unit other)
    {
        attackPower += other.attackPower;
        maxHealth += other.maxHealth / 2;
        currentHealth = maxHealth;
        Debug.Log(unitName + " merged! Stats increased.");
    }
}
