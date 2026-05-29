/// <summary>
/// A pinball bumper that bounces the ball and awards points on collision, with a visual hit effect.
/// </summary>
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [Header("Bumper")]
    public int scoreValue = 100;
    public float bounceForce = 10f;
    public Sprite hitSprite;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    /// <summary>
    /// Caches the sprite renderer and stores the original sprite.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

    /// <summary>
    /// Bounces the ball away, adds score, and shows a hit sprite temporarily.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            collision.rigidbody.AddForce(dir * bounceForce, ForceMode2D.Impulse);
            PinballManager.Instance.AddScore(scoreValue);

            if (hitSprite != null)
                spriteRenderer.sprite = hitSprite;
            Invoke(nameof(RestoreSprite), 0.1f);
        }
    }

    /// <summary>
    /// Restores the bumper's original sprite after a short delay.
    /// </summary>
    private void RestoreSprite()
    {
        spriteRenderer.sprite = originalSprite;
    }
}
