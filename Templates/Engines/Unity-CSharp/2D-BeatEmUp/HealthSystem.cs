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

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isInvincible && Time.time >= lastDamageTime + invincibilityDuration)
        {
            isInvincible = false;
        }
        UpdateUI();
    }

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

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateUI();
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

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
