using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("PowerUp Settings")]
    public PowerUpType powerUpType;
    public GameObject pickupEffect;
    public float floatAmplitude = 0.3f;
    public float floatFrequency = 1.5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition + Vector3.up *
            Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
    }

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
