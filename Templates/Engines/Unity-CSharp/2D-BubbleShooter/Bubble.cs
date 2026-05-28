using UnityEngine;

public class Bubble : MonoBehaviour
{
    [HideInInspector] public int GridX;
    [HideInInspector] public int GridY;
    [HideInInspector] public int ColorIndex;
    private BubbleShooterManager manager;
    private bool hasStopped;

    private void Awake()
    {
        manager = FindObjectOfType<BubbleShooterManager>();
    }

    public void Init(int x, int y, int color, BubbleShooterManager mgr)
    {
        GridX = x;
        GridY = y;
        ColorIndex = color;
        manager = mgr;
        hasStopped = true;
    }

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
