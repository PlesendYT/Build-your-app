/// <summary>
/// A collectible power-up that floats and grants the player a new ability on pickup.
/// </summary>
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("PowerUp Settings")]
    public PowerUpType powerUpType;
    public GameObject pickupEffect;
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 1.5f;

    private Vector3 startPosition;

    /// <summary>
    /// Records the starting position for floating animation.
    /// </summary>
    private void Start()
    {
        startPosition = transform.position;
    }

    /// <summary>
    /// Animates the power-up floating up and down.
    /// </summary>
    private void Update()
    {
        transform.position = startPosition + Vector3.up *
            Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
    }

    /// <summary>
    /// Unlocks the corresponding ability for the player and destroys the power-up.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.UnlockPowerUp(powerUpType);
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
