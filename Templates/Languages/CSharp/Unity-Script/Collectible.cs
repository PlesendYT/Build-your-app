using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int _scoreValue = 10;
    [SerializeField] private AudioClip? _pickupSound;
    [SerializeField] private GameObject? _pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var scoreManager = FindFirstObjectByType<ScoreManager>();
        if (scoreManager is not null)
            scoreManager.AddScore(_scoreValue);

        if (_pickupSound is not null)
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);

        if (_pickupEffect is not null)
            Instantiate(_pickupEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
