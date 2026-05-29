/// <summary>
/// Power-up pickup with timed effects: damage boost, speed boost, shield, health restore.
/// </summary>
using UnityEngine;

public enum PowerUpType
{
    DamageBoost,
    SpeedBoost,
    Shield,
    HealthRestore
}

public class PowerUp : MonoBehaviour
{
    [Header("Power-Up Settings")]
    public PowerUpType powerUpType = PowerUpType.DamageBoost;
    public float duration = 10f;
    public float damageMultiplier = 2f;
    public float speedMultiplier = 1.5f;
    public int healthRestoreAmount = 50;

    [Header("Visual")]
    public ParticleSystem pickupEffect;
    public Light glowLight;
    public Color[] typeColors;

    private float rotationSpeed = 90f;

    private void Start()
    {
        // Set color based on type
        if (glowLight != null && typeColors.Length > (int)powerUpType)
        {
            glowLight.color = typeColors[(int)powerUpType];
        }

        // Auto-rotate for visibility
        InvokeRepeating(nameof(Rotate), 0f, 0.05f);
    }

    private void Rotate()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out PlayerController player)) return;

        ApplyPowerUp(player);
    }

    private void ApplyPowerUp(PlayerController player)
    {
        switch (powerUpType)
        {
            case PowerUpType.DamageBoost:
                player.ApplyPowerUp(damageMultiplier, 1f, duration);
                break;

            case PowerUpType.SpeedBoost:
                player.ApplyPowerUp(1f, speedMultiplier, duration);
                break;

            case PowerUpType.Shield:
                // Invulnerability for duration
                player.ApplyPowerUp(1f, 1f, duration);
                break;

            case PowerUpType.HealthRestore:
                // Instant health restore
                player.TakeDamage(-healthRestoreAmount);
                break;
        }

        // Play pickup effect
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
