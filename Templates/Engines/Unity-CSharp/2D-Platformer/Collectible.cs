/// <summary>
/// A rotating collectible item that awards score points when picked up by the player.
/// </summary>
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Settings")]
    public int scoreValue = 100;
    public bool isPowerUp = false;
    public float rotationSpeed = 100f;

    /// <summary>
    /// Rotates the collectible each frame for visual effect.
    /// </summary>
    private void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Awards score (double if power-up) and destroys the collectible on player pickup.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPowerUp)
            {
                GameManager.Instance.AddScore(scoreValue * 2);
            }
            else
            {
                GameManager.Instance.AddScore(scoreValue);
            }
            Destroy(gameObject);
        }
    }
}
