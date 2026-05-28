using UnityEngine;

public class PinballBall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Drain"))
        {
            PinballManager.Instance.BallLost();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bumper"))
        {
            // Handled by Bumper
        }
    }
}
