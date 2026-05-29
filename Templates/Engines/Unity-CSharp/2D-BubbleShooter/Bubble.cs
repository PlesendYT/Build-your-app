/// <summary>
/// Represents a bubble in the grid: tracks grid position, color, and stops movement on collision with other bubbles or the ceiling.
/// </summary>
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [HideInInspector] public int GridX;
    [HideInInspector] public int GridY;
    [HideInInspector] public int ColorIndex;
    private BubbleShooterManager manager;
    private bool hasStopped;

    /// <summary>
    /// Finds the BubbleShooterManager on awake.
    /// </summary>
    private void Awake()
    {
        manager = FindObjectOfType<BubbleShooterManager>();
    }

    /// <summary>
    /// Initializes the bubble with grid coordinates, color index, and a reference to the manager.
    /// </summary>
    public void Init(int x, int y, int color, BubbleShooterManager mgr)
    {
        GridX = x;
        GridY = y;
        ColorIndex = color;
        manager = mgr;
        hasStopped = true;
    }

    /// <summary>
    /// Stops the bubble and notifies the manager when it collides with another bubble or the ceiling.
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasStopped && (collision.gameObject.CompareTag("Bubble") || collision.gameObject.CompareTag("Ceiling")))
        {
            hasStopped = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            manager.BubbleStopped(this);
        }
    }

    /// <summary>
    /// Stops the bubble when it triggers the ceiling collider (alternative detection).
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasStopped && other.CompareTag("Ceiling"))
        {
            hasStopped = true;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            manager.BubbleStopped(this);
        }
    }
}
