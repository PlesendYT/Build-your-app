using UnityEngine;

/// <summary>Collectible item that gives score, plays a sound, and spawns a visual effect on pickup.</summary>
public class Collectible : MonoBehaviour
{
    [SerializeField] private int _scoreValue = 10;
    [SerializeField] private AudioClip? _pickupSound;
    [SerializeField] private GameObject? _pickupEffect;

    /// <summary>When the player enters the trigger, add score, play sound, spawn effect, then destroy.</summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only react to the player
        if (!other.CompareTag("Player")) return;

        // Add score via ScoreManager
        var scoreManager = FindFirstObjectByType<ScoreManager>();
        if (scoreManager is not null)
            scoreManager.AddScore(_scoreValue);

        // Play pickup sound at this position
        if (_pickupSound is not null)
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);

        // Spawn visual effect
        if (_pickupEffect is not null)
            Instantiate(_pickupEffect, transform.position, Quaternion.identity);

        // Remove the collectible from the scene
        Destroy(gameObject);
    }
}
