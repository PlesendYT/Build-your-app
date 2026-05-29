/// <summary>
/// Manages health, damage, healing, invincibility frames, and UI display.
/// </summary>
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("UI")]
    public TMPro.TextMeshProUGUI healthText;
    public UnityEngine.UI.Slider healthSlider;

    [Header("Effects")]
    public GameObject damageEffect;
    public float invincibilityDuration = 1f;
    private bool isInvincible;
    private float lastDamageTime;

    /// <summary>
    /// Initializes health to maximum on awake.
    /// </summary>
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Tracks invincibility expiry and updates the UI each frame.
    /// </summary>
    private void Update()
    {
        if (isInvincible && Time.time >= lastDamageTime + invincibilityDuration)
        {
            isInvincible = false;
        }
        UpdateUI();
    }

    /// <summary>
    /// Reduces health by the given amount if not invincible, triggers invincibility and damage effects.
    /// </summary>
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        isInvincible = true;
        lastDamageTime = Time.time;

        if (damageEffect != null)
            Instantiate(damageEffect, transform.position, Quaternion.identity);

        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Restores health up to the maximum amount.
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateUI();
    }

    /// <summary>
    /// Deactivates the game object when health reaches zero.
    /// </summary>
    private void Die()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Refreshes the health bar and text display.
    /// </summary>
    private void UpdateUI()
    {
        if (healthText != null)
            healthText.text = currentHealth + "/" + maxHealth;
        if (healthSlider != null)
            healthSlider.value = (float)currentHealth / maxHealth;
    }

    public int GetHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
}
