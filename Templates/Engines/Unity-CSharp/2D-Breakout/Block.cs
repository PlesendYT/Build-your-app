/// <summary>
/// Represents a breakable block that changes sprite on damage and awards points when destroyed.
/// </summary>
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Block")]
    public int hitPoints = 1;
    public int scoreValue = 50;
    public Sprite[] damageSprites;
    private SpriteRenderer spriteRenderer;
    private int currentHits;

    /// <summary>
    /// Caches the sprite renderer component.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Handles ball collision: increments hit count, destroys block or updates damage sprite.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            currentHits++;
            if (currentHits >= hitPoints)
            {
                BreakoutManager.Instance.BlockDestroyed(scoreValue);
                Destroy(gameObject);
            }
            else if (damageSprites.Length > 0)
            {
                int idx = Mathf.Min(currentHits - 1, damageSprites.Length - 1);
                spriteRenderer.sprite = damageSprites[idx];
            }
        }
    }
}
