using UnityEngine;

public class Bumper : MonoBehaviour
{
    [Header("Bumper")]
    public int scoreValue = 100;
    public float bounceForce = 10f;
    public Sprite hitSprite;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
    }

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

    private void RestoreSprite()
    {
        spriteRenderer.sprite = originalSprite;
    }
}
