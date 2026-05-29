/// <summary>
/// Represents the pinball ball, triggering ball loss when it enters the drain zone.
/// </summary>
using UnityEngine;

public class PinballBall : MonoBehaviour
{
    /// <summary>
    /// Detects when the ball falls into the drain and notifies the PinballManager.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Drain"))
        {
            PinballManager.Instance.BallLost();
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Bumper collision is handled by the Bumper component.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bumper"))
        {
            // Handled by Bumper
        }
    }
}
